using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// プロシージャルフィールド生成を管理するクラス
/// </summary>
public class FieldGenerator : MonoBehaviour
{
    #region Constants
    private const int SMOOTHING_ITERATIONS = 3; // スムージングの反復回数
    private const int SURROUNDING_GROUND_THRESHOLD = 4; // 周囲の地面タイル数の閾値
    private const float GATE_OBJECT_Y_OFFSET = 0.25f;
    private const float OBJECT_POSITION_OFFSET = 0.5f;
    private const int TREASURE_BOX_COUNT = 3;

    // 方向定数
    private static readonly Vector2Int[] DIRECTIONS =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };
    #endregion

    #region Serialized Fields
    [SerializeField] private AgeTimePanel ageTimePanel;

    [Header("DefaultField Settings")]
    [SerializeField] private FieldBase defaultFieldBase;
    [SerializeField] private PointDatabase pointDatabase;
    [SerializeField] private GatePrefab gatePrefab;
    [SerializeField] private TreasureBoxPrefab treasureBoxObject;
    [SerializeField] private QuestPrefab questPrefab;
    [SerializeField] private PointPrefab pointPrefab;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tilemap encountTilemap;

    [Header("CurrentField Settings")]
    private FieldBase currentFieldBase;

    [Header("FieldMap Settings")]
    private TileBase groundTile;
    private TileBase areaTile;
    private GameObject[] fieldObjectPrefabs;
    private int[,] mapBase;
    private int[,] areaMapBase;
    private HashSet<Vector2Int> placedGates = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> placedObjects = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> placedPoints = new HashSet<Vector2Int>();
    private System.Random consistentRandom;

    // ゲート位置
    private readonly Dictionary<DirectionType, Vector2Int> gatePositions = new Dictionary<DirectionType, Vector2Int>();
    #endregion

    #region Public Properties
    public Tilemap Tilemap => tilemap;
    public Tilemap EncountTilemap => encountTilemap;
    #endregion

    #region Public Methods
    /// <summary>
    /// フィールドを設定して生成する
    /// </summary>
    /// <param name="fieldBase">フィールドデータ</param>
    /// <param name="fieldTileSet">タイルセット</param>
    public void SetField(FieldBase fieldBase, FieldTileSet fieldTileSet)
    {
        if (fieldTileSet == null)
        {
            UnityEngine.Debug.LogError("FieldTileSet is null!");
            return;
        }
        currentFieldBase = fieldBase ?? defaultFieldBase;
        string seed = "";
        if (currentFieldBase.FieldName != null && currentFieldBase.FieldName != "")
        {
            seed = currentFieldBase.Seed;
        }
        else
        {
            seed = currentFieldBase.currentPosition.x + "," + currentFieldBase.currentPosition.y;
        }
        Random.InitState(seed.GetHashCode());
        consistentRandom = new System.Random(seed.GetHashCode());
        InitializeField(currentFieldBase, fieldTileSet);
        GenerateField();
    }

    /// <summary>
    /// プレイヤーの入場位置を取得する
    /// </summary>
    /// <param name="direction">移動方向</param>
    /// <returns>入場位置のタイル座標</returns>
    public Vector3Int GetEntrancePosition(DirectionType direction)
    {
        if (direction == DirectionType.Other)
        {
            // 進行方向が指定されていない場合はマップの中心を返す
            return new Vector3Int(currentFieldBase.FieldWidth / 2, currentFieldBase.FieldHeight / 2, 0);
        }
        Vector2Int targetPos = GetGatePosition(direction);
        // targetPosから２マス離れた場所の地面がある場所を返す
        for (int x = -2; x <= 2; x++)
        {
            for (int y = -2; y <= 2; y++)
            {
                if ((-1 <= x && x <= 1) && (-1 <= y && y <= 1)) continue; // 自分自身とその周囲は除外

                Vector2Int checkPos = targetPos + new Vector2Int(x, y);
                if (IsValidMapPosition(checkPos.x, checkPos.y) && IsGroundOrArea(checkPos))
                {
                    targetPos = checkPos;
                    targetPos += new Vector2Int(0, 1);
                    return ConvertToTilePosition(targetPos);
                }
            }
        }
        return ConvertToTilePosition(targetPos);
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// フィールドを生成する
    /// </summary>
    private void GenerateField()
    {
        if (currentFieldBase == null)
        {
            UnityEngine.Debug.LogError("FieldBase is not set! Call SetField first.");
            return;
        }

        ClearField();
        CreateBaseMap();
        CreateAllGates();
        RenderField();
        CreateFieldObjects();
        CreatePointObjects();
        CreateQuestObjects();
        CreateTreasureBoxObjects();
    }
    #endregion

    #region MapRender Methods

    /// <summary>
    /// フィールドをクリアする
    /// </summary>
    private void ClearField()
    {
        if (tilemap != null)
        {
            tilemap.ClearAllTiles();
            encountTilemap.ClearAllTiles();
        }

        InitializeMaps();
        placedGates.Clear();
        gatePositions.Clear();
        DestroyFieldObjects();
    }

    /// <summary>
    /// フィールドマップとエリアマップを作成
    /// </summary>
    /// <returns>入場位置のタイル座標</returns>
    private void CreateBaseMap()
    {
        mapBase = GenerateGroundMap(currentFieldBase.GroundFillPercent);
        areaMapBase = GenerateGroundMap(currentFieldBase.AreaFillPercent);
        ProcessTerrainMaps();
    }

    /// <summary>
    /// フィールドの初期化
    /// </summary>
    private void InitializeField(FieldBase fieldBase, FieldTileSet fieldTileSet)
    {
        currentFieldBase = fieldBase ?? defaultFieldBase;
        this.groundTile = fieldTileSet.GroundTile ?? defaultFieldBase.FieldTileSet.GroundTile;
        this.areaTile = fieldTileSet.AreaTile ?? defaultFieldBase.FieldTileSet.AreaTile;

        // フィールドのパラメータを設定
        this.fieldObjectPrefabs = new GameObject[0];
        fieldObjectPrefabs = fieldTileSet.ObjectPrefabs ?? defaultFieldBase.FieldTileSet.ObjectPrefabs;
    }

    /// <summary>
    /// 地面マップを生成
    /// </summary>
    private int[,] GenerateGroundMap(float fillPercent)
    {
        int[,] draftMap = new int[currentFieldBase.FieldWidth, currentFieldBase.FieldHeight];
        for (int x = 0; x < currentFieldBase.FieldWidth; x++)
        {
            for (int y = 0; y < currentFieldBase.FieldHeight; y++)
            {
                if (Random.value < fillPercent)
                {
                    draftMap[x, y] = (int)TileType.Ground;
                }
                else
                {
                    draftMap[x, y] = (int)TileType.None; // 空タイルを設定
                }
            }
        }
        return draftMap;
    }

    /// <summary>
    /// 地形マップを処理（スムージングとマージ）
    /// </summary>
    private void ProcessTerrainMaps()
    {
        mapBase = SmoothMap(mapBase);
        areaMapBase = SmoothMap(areaMapBase);
        MergeAreaMap();
    }

    /// <summary>
    /// マップをスムージング
    /// </summary>
    private int[,] SmoothMap(int[,] draftMap)
    {
        for (int i = 0; i < SMOOTHING_ITERATIONS; i++)
        {
            for (int x = 0; x < currentFieldBase.FieldWidth; x++)
            {
                for (int y = 0; y < currentFieldBase.FieldHeight; y++)
                {
                    int surroundingGroundCount = GetSurroundingGroundCount(x, y, draftMap);

                    if (surroundingGroundCount >= SURROUNDING_GROUND_THRESHOLD)
                    {
                        draftMap[x, y] = (int)TileType.Ground;
                    }
                    else
                    {
                        draftMap[x, y] = (int)TileType.None; // 空タイルを設定
                    }
                }
            }
        }
        return draftMap;
    }

    /// <summary>
    /// エリアマップをベースマップにマージ
    /// </summary>
    private void MergeAreaMap()
    {
        for (int x = 0; x < currentFieldBase.FieldWidth; x++)
        {
            for (int y = 0; y < currentFieldBase.FieldHeight; y++)
            {
                if (areaMapBase[x, y] == (int)TileType.Ground && mapBase[x, y] == (int)TileType.Ground)
                {
                    mapBase[x, y] = (int)TileType.Area;
                }
            }
        }
    }

    /// <summary>
    /// 周囲の地面タイル数を取得
    /// </summary>
    private int GetSurroundingGroundCount(int gridX, int gridY, int[,] draftMap)
    {
        int count = 0;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = gridX + x;
                int checkY = gridY + y;

                if (IsValidMapPosition(checkX, checkY))
                {
                    count += draftMap[checkX, checkY];
                }
            }
        }

        return count;
    }

    /// <summary>
    /// フィールドのタイルをレンダリング
    /// </summary>
    private void RenderField()
    {
        for (int x = 0; x < currentFieldBase.FieldWidth; x++)
        {
            for (int y = 0; y < currentFieldBase.FieldHeight; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                TileType tileType = (TileType)mapBase[x, y];
                TileBase tileToPlace = GetTileForType(tileType);

                if (tileType == TileType.Area)
                {
                    encountTilemap.SetTile(tilePosition, tileToPlace);
                }

                if (tileToPlace != null)
                {
                    tilemap.SetTile(tilePosition, tileToPlace);
                }
            }
        }
    }

    /// <summary>
    /// タイプに対応するタイルを取得
    /// </summary>
    private TileBase GetTileForType(TileType tileType)
    {
        return tileType switch
        {
            TileType.Ground => groundTile,
            TileType.Area => areaTile,
            TileType.Gate => groundTile,
            _ => null
        };
    }

    /// <summary>
    /// マップを初期化
    /// </summary>
    private void InitializeMaps()
    {
        mapBase = new int[currentFieldBase.FieldWidth, currentFieldBase.FieldHeight];
        areaMapBase = new int[currentFieldBase.FieldWidth, currentFieldBase.FieldHeight];
    }

    #endregion

    #region GateObject Methods

    /// <summary>
    /// 有効なマップ位置かチェック
    /// </summary>
    private bool IsValidMapPosition(int x, int y)
    {
        return x >= 0 && x < currentFieldBase.FieldWidth && y >= 0 && y < currentFieldBase.FieldHeight;
    }

    /// <summary>
    /// ゲート位置を取得
    /// </summary>
    private Vector2Int GetGatePosition(DirectionType direction)
    {
        if (gatePositions.TryGetValue(direction, out Vector2Int gatePos))
        {
            return gatePos;
        }

        // デフォルトは下ゲート
        return gatePositions.GetValueOrDefault(DirectionType.Bottom, Vector2Int.zero);
    }

    /// <summary>
    /// 地面または草かチェック
    /// </summary>
    private bool IsGroundOrArea(Vector2Int pos)
    {
        return mapBase[pos.x, pos.y] == (int)TileType.Ground ||
               mapBase[pos.x, pos.y] == (int)TileType.Area;
    }

    /// <summary>
    /// すべてのゲートを作成
    /// </summary>
    private void CreateAllGates()
    {
        if (currentFieldBase.isTopOpen)
        {
            Vector2Int topGatePosition = new Vector2Int(Random.Range(currentFieldBase.FieldWidth / 4, currentFieldBase.FieldWidth * 3 / 4), currentFieldBase.FieldHeight - 1);
            CreateGate(topGatePosition, DirectionType.Top);
            gatePositions[DirectionType.Top] = topGatePosition;
        }

        if (currentFieldBase.isBottomOpen)
        {
            Vector2Int bottomGatePosition = new Vector2Int(Random.Range(currentFieldBase.FieldWidth / 4, currentFieldBase.FieldWidth * 3 / 4), 0);
            CreateGate(bottomGatePosition, DirectionType.Bottom);
            gatePositions[DirectionType.Bottom] = bottomGatePosition;
        }

        if (currentFieldBase.isRightOpen)
        {
            Vector2Int rightGatePosition = new Vector2Int(currentFieldBase.FieldWidth - 1, Random.Range(currentFieldBase.FieldHeight / 4, currentFieldBase.FieldHeight * 3 / 4));
            CreateGate(rightGatePosition, DirectionType.Right);
            gatePositions[DirectionType.Right] = rightGatePosition;
        }

        if (currentFieldBase.isLeftOpen)
        {
            Vector2Int leftGatePosition = new Vector2Int(0, Random.Range(currentFieldBase.FieldHeight / 4, currentFieldBase.FieldHeight * 3 / 4));
            CreateGate(leftGatePosition, DirectionType.Left);
            gatePositions[DirectionType.Left] = leftGatePosition;
        }
    }

    /// <summary>
    /// ゲートを作成
    /// </summary>
    private void CreateGate(Vector2Int entryPosition, DirectionType direction)
    {
        mapBase[entryPosition.x, entryPosition.y] = (int)TileType.Gate;

        CreateGatePathfinding(entryPosition);
        CreateGateObject(entryPosition, direction);
    }

    /// <summary>
    /// ゲートから地面への経路を作成
    /// </summary>
    private void CreateGatePathfinding(Vector2Int entry)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        queue.Enqueue(entry);
        visited.Add(entry);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            if (IsGroundOrArea(current) && !placedGates.Contains(current))
            {
                CreatePath(current, cameFrom);
                break;
            }

            foreach (var dir in DIRECTIONS)
            {
                Vector2Int neighbor = current + dir;
                if (IsValidMapPosition(neighbor.x, neighbor.y) && !visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }
    }

    /// <summary>
    /// パスを作成
    /// </summary>
    private void CreatePath(Vector2Int start, Dictionary<Vector2Int, Vector2Int> cameFrom)
    {
        Vector2Int pathPos = start;
        while (cameFrom.ContainsKey(pathPos))
        {
            mapBase[pathPos.x, pathPos.y] = (int)TileType.Ground;
            pathPos = cameFrom[pathPos];
        }
    }

    /// <summary>
    /// ゲートオブジェクトを作成
    /// </summary>
    private void CreateGateObject(Vector2Int entry, DirectionType direction)
    {
        if (gatePrefab == null) return;

        Vector3Int tilePosition = ConvertToTilePosition(entry);
        Vector3 worldPos = tilemap.GetCellCenterWorld(tilePosition) + new Vector3(0f, GATE_OBJECT_Y_OFFSET, 0f);
        GatePrefab instantiatedGatePrefab = Instantiate(gatePrefab, worldPos, Quaternion.identity, this.transform);
        instantiatedGatePrefab.directionType = direction;
    }

    #endregion

    #region FieldObject Methods

    /// <summary>
    /// リストをシャッフル（Fisher-Yates shuffle）
    /// </summary>
    private void ShuffleList<T>(List<T> list, System.Random random)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    /// <summary>
    /// オブジェクトを配置可能かチェック
    /// </summary>
    private bool CanPlaceObject(int x, int y)
    {
        //placedGates.Contains(new Vector2Int(x, y))は、すでにゲートが配置されている場所を除外
        if (placedGates.Contains(new Vector2Int(x, y)) || placedObjects.Contains(new Vector2Int(x, y)) || placedPoints.Contains(new Vector2Int(x, y)))
        {
            return false; // すでにオブジェクトが配置されている場所は除外
        }
        // 周囲が地面または草であることを確認
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue; // 自分自身は除外
                int checkX = x + dx;
                int checkY = y + dy;

                if (!IsValidMapPosition(checkX, checkY) || !IsGroundOrArea(new Vector2Int(checkX, checkY)))
                {
                    return false; // 周囲に地面またはエリアがない場合は配置不可
                }
            }
        }
        return mapBase[x, y] == (int)TileType.Ground || mapBase[x, y] == (int)TileType.Area;
    }


    /// <summary>
    /// オブジェクトのワールド座標を取得
    /// </summary>
    private Vector3 GetObjectWorldPosition(int x, int y)
    {
        Vector3Int tilePos = new Vector3Int(x, y, 0);
        return tilemap.CellToWorld(tilePos) +
               new Vector3(OBJECT_POSITION_OFFSET, OBJECT_POSITION_OFFSET, 0f);
    }

    /// <summary>
    /// フィールドオブジェクトを破棄
    /// </summary>
    private void DestroyFieldObjects()
    {
        DestroyObjectsByTag("FieldObject");
        DestroyObjectsByTag("Gate");
    }

    /// <summary>
    /// 指定されたタグのオブジェクトを破棄
    /// </summary>
    private void DestroyObjectsByTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (var obj in objects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
    }

    /// <summary>
    /// ワールド座標をタイル座標に変換
    /// </summary>
    private Vector3Int ConvertToTilePosition(Vector2Int worldPos)
    {
        return new Vector3Int(worldPos.x, worldPos.y, 0);
    }

    /// <summary>
    /// オブジェクト配置用のシードを生成
    /// </summary>
    private int GenerateObjectSeed()
    {
        // currentFieldBaseの位置とオブジェクト数を組み合わせてシードを生成
        string seedString = "";
        if (currentFieldBase.FieldName != null && currentFieldBase.FieldName != "")
        {
            seedString = currentFieldBase.Seed;
        }
        else
        {
            seedString = $"{currentFieldBase.Position.x},{currentFieldBase.Position.y}";
        }
        return seedString.GetHashCode();
    }

    /// <summary>
    /// ポイント配置用のシードを生成
    /// </summary>
    private int GeneratePointSeed()
    {
        // currentFieldBaseの位置とポイント数を組み合わせてシードを生成
        string seedString = $"{currentFieldBase.Position.x},{currentFieldBase.Position.y},points,{currentFieldBase.Points?.Count ?? 0}";
        return seedString.GetHashCode();
    }

    /// <summary>
    /// 配置可能な位置のリストを取得
    /// </summary>
    private List<Vector2Int> GetValidObjectPositions()
    {
        List<Vector2Int> validPositions = new List<Vector2Int>();

        for (int x = 0; x < currentFieldBase.FieldWidth; x++)
        {
            for (int y = 0; y < currentFieldBase.FieldHeight; y++)
            {
                if (CanPlaceObject(x, y))
                {
                    validPositions.Add(new Vector2Int(x, y));
                }
            }
        }

        return validPositions;
    }

    /// <summary>
    /// 基本的なオブジェクト配置可能性をチェック
    /// </summary>
    private bool CanPlaceObjectBasic(int x, int y)
    {
        // ゲートが配置されている場所は除外
        if (placedGates.Contains(new Vector2Int(x, y)))
        {
            return false;
        }

        // 周囲が地面または草であることを確認
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue; // 自分自身は除外
                int checkX = x + dx;
                int checkY = y + dy;

                if (!IsValidMapPosition(checkX, checkY) || !IsGroundOrArea(new Vector2Int(checkX, checkY)))
                {
                    return false; // 周囲に地面または草がない場合は配置不可
                }
            }
        }
        return mapBase[x, y] == (int)TileType.Ground || mapBase[x, y] == (int)TileType.Area;
    }

    /// <summary>
    /// フィールドオブジェクトを作成
    /// </summary>
    private void CreateFieldObjects()
    {
        if (fieldObjectPrefabs == null || fieldObjectPrefabs.Length == 0) return;

        // 一意のシードを使用してランダムな位置を生成
        System.Random objectRandom = new System.Random(GenerateObjectSeed());
        List<Vector2Int> validPositions = GetValidObjectPositions();

        if (validPositions.Count == 0) return;

        // シャッフルして一貫性のある順序を確保
        ShuffleList(validPositions, objectRandom);

        int actualObjectCount = Mathf.Min(currentFieldBase.ObjectCount, validPositions.Count);

        for (int i = 0; i < actualObjectCount; i++)
        {
            Vector2Int pos = validPositions[i];
            Vector3 position = GetObjectWorldPosition(pos.x, pos.y);
            GameObject prefab = fieldObjectPrefabs[objectRandom.Next(0, fieldObjectPrefabs.Length)];
            Instantiate(prefab, position, Quaternion.identity, this.transform);
            placedObjects.Add(pos);
        }
    }

    /// <summary>
    /// ポイントオブジェクトを作成
    /// </summary>
    private void CreatePointObjects()
    {
        if (currentFieldBase.Points == null || currentFieldBase.Points.Count == 0) return;

        // ポイント専用のシードを使用
        System.Random pointRandom = new System.Random(GeneratePointSeed());
        List<Vector2Int> validPositions = GetValidObjectPositions();

        if (validPositions.Count == 0) return;

        // シャッフルして一貫性のある順序を確保
        ShuffleList(validPositions, pointRandom);

        int actualPointCount = Mathf.Min(currentFieldBase.Points.Count, validPositions.Count);

        for (int i = 0; i < actualPointCount; i++)
        {
            Vector2Int pos = validPositions[i];
            Vector3 position = GetObjectWorldPosition(pos.x, pos.y);

            // インスタンス化したオブジェクトを取得
            PointPrefab instantiatedPointObject = Instantiate(pointPrefab, position, Quaternion.identity, this.transform);
            try
            {
                // PointBaseをPointに実体化させて格納
                Point point = pointDatabase.GetPoint(currentFieldBase.Points[i]);
                instantiatedPointObject.SetPoint(point);
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError($"Failed to convert PointBase to Point: {e.Message}");
            }

            placedPoints.Add(pos);
        }
    }

    /// <summary>
    /// クエストオブジェクトを作成
    /// </summary>
    private void CreateQuestObjects()
    {
        List<Quest> questList = new List<Quest>();

        questList = QuestDatabase.Instance.GetActiveQuestByField(ageTimePanel.ageTime, currentFieldBase.fieldType);

        if (questList == null || questList.Count == 0) return;

        // クエスト専用のシードを使用
        System.Random questRandom = new System.Random(GenerateObjectSeed());
        List<Vector2Int> validPositions = GetValidObjectPositions();

        if (validPositions.Count == 0) return;

        // シャッフルして一貫性のある順序を確保
        ShuffleList(validPositions, questRandom);

        int actualQuestCount = Mathf.Min(questList.Count, validPositions.Count);

        for (int i = 0; i < actualQuestCount; i++)
        {
            Vector2Int pos = validPositions[i];
            Vector3 position = GetObjectWorldPosition(pos.x, pos.y);

            // インスタンス化したオブジェクトを取得
            QuestPrefab instantiatedQuestObject = Instantiate(questPrefab, position, Quaternion.identity, this.transform);
            try
            {
                instantiatedQuestObject.SetQuest(questList[i]);
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError($"Failed to convert QuestBase to Quest: {e.Message}");
            }

            placedObjects.Add(pos);
        }
    }

    /// <summary>
    /// 宝箱オブジェクトを作成
    /// </summary>
    private void CreateTreasureBoxObjects()
    {
        int placed = 0;
        System.Random trueRandom = new System.Random();

        int treasureCount = trueRandom.Next(0, TREASURE_BOX_COUNT);

        while (placed < treasureCount)
        {
            int x = trueRandom.Next(0, currentFieldBase.FieldWidth);
            int y = trueRandom.Next(0, currentFieldBase.FieldHeight);

            if (CanPlaceObject(x, y))
            {
                Vector3 fieldPos = GetObjectWorldPosition(x, y);
                Instantiate(treasureBoxObject, fieldPos, Quaternion.identity, this.transform);
                placedGates.Add(new Vector2Int(x, y));
                placed++;
            }
        }
    }
    #endregion
}
