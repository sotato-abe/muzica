using System.Collections;
using System.Collections.Generic;
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
    [Header("Field Configuration")]
    [SerializeField] private FieldData defaultFieldData;
    [SerializeField] private GameObject gateObject;
    [SerializeField] private GameObject treasureBoxObject;
    [SerializeField] private GameObject pointObject;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tilemap encountTilemap;

    [Header("Tiles")]
    private TileBase groundTile;
    private TileBase grassTile;
    private TileBase gateTile;

    [Header("Objects")]
    [SerializeField] private GameObject[] objectPrefabs;
    #endregion

    #region Private Fields
    private int[,] mapBase;
    private int[,] areaMapBase;
    private int width;
    private int height;
    private float groundFillPercent;
    private float areaFillPercent;
    private int objectCount;
    private HashSet<Vector2Int> placedGates = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> placedObjects = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> placedPoints = new HashSet<Vector2Int>();
    private System.Random consistentRandom;

    // ゲート位置
    private readonly Dictionary<Vector2Int, Vector2Int> gatePositions = new Dictionary<Vector2Int, Vector2Int>();
    #endregion

    #region Public Properties
    public Tilemap Tilemap => tilemap;
    public Tilemap EncountTilemap => encountTilemap;
    public FieldData fieldData;
    #endregion

    #region Public Methods
    /// <summary>
    /// フィールドを設定して生成する
    /// </summary>
    /// <param name="fieldData">フィールドデータ</param>
    /// <param name="fieldTileSet">タイルセット</param>
    public void SetField(FieldData fieldData, FieldTileSet fieldTileSet)
    {
        if (fieldTileSet == null)
        {
            Debug.LogError("FieldTileSet is null!");
            return;
        }
        string seed = fieldData.currentPosition.x + "," + fieldData.currentPosition.y;
        Random.InitState(seed.GetHashCode());
        consistentRandom = new System.Random(seed.GetHashCode());
        InitializeField(fieldData, fieldTileSet);
        GenerateField();
    }

    /// <summary>
    /// フィールドを生成する
    /// </summary>
    public void GenerateField()
    {
        if (fieldData == null)
        {
            Debug.LogError("FieldData is not set! Call SetField first.");
            return;
        }

        ClearField();
        GenerateTerrainMaps();
        ProcessTerrainMaps();
        CreateAllGates();
        RenderField();
        CreateFieldObjects();
        CreatePointObjects();
        CreateTreasureBoxObjects();
    }

    /// <summary>
    /// プレイヤーの入場位置を取得する
    /// </summary>
    /// <param name="direction">移動方向</param>
    /// <returns>入場位置のタイル座標</returns>
    public Vector3Int GetEntrancePosition(Vector2Int direction)
    {
        if (direction == Vector2Int.zero)
        {
            // 進行方向が指定されていない場合はマップの中心を返す
            return new Vector3Int(width / 2, height / 2, 0);
        }
        Vector2Int targetPos = GetGatePosition(direction);
        // targetPosから２マス離れた場所の地面がある場所を返す
        for (int x = -2; x <= 2; x++)
        {
            for (int y = -2; y <= 2; y++)
            {
                if ((-1 <= x && x <= 1) && (-1 <= y && y <= 1)) continue; // 自分自身とその周囲は除外

                Vector2Int checkPos = targetPos + new Vector2Int(x, y);
                if (IsValidMapPosition(checkPos.x, checkPos.y) && IsGroundOrGrass(checkPos))
                {
                    targetPos = checkPos;
                    targetPos += new Vector2Int(0, 1);
                    return ConvertToTilePosition(targetPos);
                }
            }
        }
        return ConvertToTilePosition(targetPos);
    }

    /// <summary>
    /// フィールドをクリアする
    /// </summary>
    public void ClearField()
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
    #endregion

    #region Private Methods
    /// <summary>
    /// フィールドの初期化
    /// </summary>
    private void InitializeField(FieldData fieldData, FieldTileSet fieldTileSet)
    {
        this.fieldData = fieldData ?? defaultFieldData;
        this.groundTile = fieldTileSet.GroundTile ?? defaultFieldData.FieldTileSet.GroundTile;
        this.grassTile = fieldTileSet.GrassTile ?? defaultFieldData.FieldTileSet.GrassTile;

        // フィールドのパラメータを設定
        width = this.fieldData.FieldWidth;
        height = this.fieldData.FieldHeight;
        groundFillPercent = this.fieldData.GroundFillPercent;
        areaFillPercent = this.fieldData.AreaFillPercent;
        objectCount = this.fieldData.ObjectCount;
        this.objectPrefabs = new GameObject[0];
        objectPrefabs = fieldTileSet.ObjectPrefabs ?? defaultFieldData.FieldTileSet.ObjectPrefabs;
    }

    /// <summary>
    /// 地形マップを生成
    /// </summary>
    private void GenerateTerrainMaps()
    {
        mapBase = GenerateGroundMap(new int[width, height], groundFillPercent);
        areaMapBase = GenerateGroundMap(new int[width, height], areaFillPercent);
    }

    /// <summary>
    /// 地面マップを生成
    /// </summary>
    private int[,] GenerateGroundMap(int[,] draftMap, float fillPercent)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
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
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
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
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (areaMapBase[x, y] == (int)TileType.Ground && mapBase[x, y] == (int)TileType.Ground)
                {
                    mapBase[x, y] = (int)TileType.Grass;
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
    /// 有効なマップ位置かチェック
    /// </summary>
    private bool IsValidMapPosition(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    /// <summary>
    /// すべてのゲートを作成
    /// </summary>
    private void CreateAllGates()
    {
        if (fieldData.isTopOpen)
        {
            Vector2Int topGate = new Vector2Int(Random.Range(width / 4, width * 3 / 4), height - 1);
            CreateGate(topGate, Vector2Int.up);
            gatePositions[Vector2Int.up] = topGate;
        }

        if (fieldData.isBottomOpen)
        {
            Vector2Int bottomGate = new Vector2Int(Random.Range(width / 4, width * 3 / 4), 0);
            CreateGate(bottomGate, Vector2Int.down);
            gatePositions[Vector2Int.down] = bottomGate;
        }

        if (fieldData.isRightOpen)
        {
            Vector2Int rightGate = new Vector2Int(width - 1, Random.Range(height / 4, height * 3 / 4));
            CreateGate(rightGate, Vector2Int.right);
            gatePositions[Vector2Int.right] = rightGate;
        }

        if (fieldData.isLeftOpen)
        {
            Vector2Int leftGate = new Vector2Int(0, Random.Range(height / 4, height * 3 / 4));
            CreateGate(leftGate, Vector2Int.left);
            gatePositions[Vector2Int.left] = leftGate;
        }
    }

    /// <summary>
    /// ゲートを作成
    /// </summary>
    private void CreateGate(Vector2Int entry, Vector2Int direction)
    {
        mapBase[entry.x, entry.y] = (int)TileType.Gate;

        CreateGatePathfinding(entry);
        CreateGateObject(entry, direction);
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

            if (IsGroundOrGrass(current) && !placedGates.Contains(current))
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
    /// 地面または草かチェック
    /// </summary>
    private bool IsGroundOrGrass(Vector2Int pos)
    {
        return mapBase[pos.x, pos.y] == (int)TileType.Ground ||
               mapBase[pos.x, pos.y] == (int)TileType.Grass;
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
    private void CreateGateObject(Vector2Int entry, Vector2Int direction)
    {
        if (gateObject == null) return;

        Vector3Int tilePosition = ConvertToTilePosition(entry);
        Vector3 worldPos = tilemap.GetCellCenterWorld(tilePosition) + new Vector3(0f, GATE_OBJECT_Y_OFFSET, 0f);
        GameObject gateObj = Instantiate(gateObject, worldPos, Quaternion.identity, this.transform);

        GateTrigger gateTrigger = gateObj.GetComponent<GateTrigger>();
        if (gateTrigger != null)
        {
            gateTrigger.direction = direction;
        }
    }

    /// <summary>
    /// フィールドオブジェクトを作成
    /// </summary>
    private void CreateFieldObjects()
    {
        if (objectPrefabs == null || objectPrefabs.Length == 0) return;

        // 一意のシードを使用してランダムな位置を生成
        System.Random objectRandom = new System.Random(GenerateObjectSeed());
        List<Vector2Int> validPositions = GetValidObjectPositionsForObjects();

        if (validPositions.Count == 0) return;

        // シャッフルして一貫性のある順序を確保
        ShuffleList(validPositions, objectRandom);

        int actualObjectCount = Mathf.Min(objectCount, validPositions.Count);

        for (int i = 0; i < actualObjectCount; i++)
        {
            Vector2Int pos = validPositions[i];
            Vector3 position = GetObjectWorldPosition(pos.x, pos.y);
            GameObject prefab = objectPrefabs[objectRandom.Next(0, objectPrefabs.Length)];
            Instantiate(prefab, position, Quaternion.identity, this.transform);
            placedObjects.Add(pos);
        }
    }

    /// <summary>
    /// ポイントオブジェクトを作成
    /// </summary>
    private void CreatePointObjects()
    {
        if (fieldData.Points == null || fieldData.Points.Count == 0) return;

        // ポイント専用のシードを使用
        System.Random pointRandom = new System.Random(GeneratePointSeed());
        List<Vector2Int> validPositions = GetValidObjectPositionsForPoints();

        if (validPositions.Count == 0) return;

        // シャッフルして一貫性のある順序を確保
        ShuffleList(validPositions, pointRandom);

        int actualPointCount = Mathf.Min(fieldData.Points.Count, validPositions.Count);

        for (int i = 0; i < actualPointCount; i++)
        {
            Vector2Int pos = validPositions[i];
            Vector3 position = GetObjectWorldPosition(pos.x, pos.y);

            // インスタンス化したオブジェクトを取得
            GameObject instantiatedPointObject = Instantiate(pointObject, position, Quaternion.identity, this.transform);

            // インスタンス化したオブジェクトからPointTriggerコンポーネントを取得
            PointTrigger pointTrigger = instantiatedPointObject.GetComponent<PointTrigger>();
            if (pointTrigger != null)
            {
                try
                {
                    // PointBaseをPointに実体化させて格納
                    pointTrigger.point = fieldData.Points[i].ToPoint(); // PointBaseからPointに変換
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Failed to convert PointBase to Point: {e.Message}");
                }
            }
            else
            {
                Debug.LogError($"PointTrigger component not found on instantiated point object at position {pos}");
            }

            placedPoints.Add(pos);
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
            int x = trueRandom.Next(0, width);
            int y = trueRandom.Next(0, height);

            if (CanPlaceObject(x, y))
            {
                Vector3 fieldPos = GetObjectWorldPosition(x, y);
                Instantiate(treasureBoxObject, fieldPos, Quaternion.identity, this.transform);
                placedGates.Add(new Vector2Int(x, y));
                placed++;
            }
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

                if (!IsValidMapPosition(checkX, checkY) || !IsGroundOrGrass(new Vector2Int(checkX, checkY)))
                {
                    return false; // 周囲に地面または草がない場合は配置不可
                }
            }
        }
        return mapBase[x, y] == (int)TileType.Ground || mapBase[x, y] == (int)TileType.Grass;
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
    /// フィールドのタイルをレンダリング
    /// </summary>
    private void RenderField()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                TileType tileType = (TileType)mapBase[x, y];
                TileBase tileToPlace = GetTileForType(tileType);

                if (tileType == TileType.Grass)
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
            TileType.Grass => grassTile,
            TileType.Gate => groundTile,
            _ => null
        };
    }

    /// <summary>
    /// マップを初期化
    /// </summary>
    private void InitializeMaps()
    {
        mapBase = new int[width, height];
        areaMapBase = new int[width, height];
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
    /// ゲート位置を取得
    /// </summary>
    private Vector2Int GetGatePosition(Vector2Int direction)
    {
        Vector2Int oppositeDirection = GetOppositeDirection(direction);

        if (gatePositions.TryGetValue(oppositeDirection, out Vector2Int gatePos))
        {
            return gatePos;
        }

        // デフォルトは下ゲート
        return gatePositions.GetValueOrDefault(Vector2Int.down, Vector2Int.zero);
    }

    /// <summary>
    /// 反対方向を取得
    /// </summary>
    private Vector2Int GetOppositeDirection(Vector2Int direction)
    {
        return direction switch
        {
            var d when d == Vector2Int.up => Vector2Int.down,
            var d when d == Vector2Int.down => Vector2Int.up,
            var d when d == Vector2Int.left => Vector2Int.right,
            var d when d == Vector2Int.right => Vector2Int.left,
            _ => Vector2Int.down
        };
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
        // fieldDataの位置とオブジェクト数を組み合わせてシードを生成
        string seedString = $"{fieldData.Position.x},{fieldData.Position.y},objects,{objectCount}";
        return seedString.GetHashCode();
    }

    /// <summary>
    /// ポイント配置用のシードを生成
    /// </summary>
    private int GeneratePointSeed()
    {
        // fieldDataの位置とポイント数を組み合わせてシードを生成
        string seedString = $"{fieldData.Position.x},{fieldData.Position.y},points,{fieldData.Points?.Count ?? 0}";
        return seedString.GetHashCode();
    }

    /// <summary>
    /// 配置可能な位置のリストを取得
    /// </summary>
    private List<Vector2Int> GetValidObjectPositions()
    {
        List<Vector2Int> validPositions = new List<Vector2Int>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
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
    /// オブジェクト配置用の有効な位置リストを取得（ゲートのみ考慮）
    /// </summary>
    private List<Vector2Int> GetValidObjectPositionsForObjects()
    {
        List<Vector2Int> validPositions = new List<Vector2Int>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (CanPlaceObjectBasic(x, y))
                {
                    validPositions.Add(new Vector2Int(x, y));
                }
            }
        }

        return validPositions;
    }

    /// <summary>
    /// ポイント配置用の有効な位置リストを取得（ゲートのみ考慮）
    /// </summary>
    private List<Vector2Int> GetValidObjectPositionsForPoints()
    {
        List<Vector2Int> validPositions = new List<Vector2Int>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (CanPlaceObjectBasic(x, y))
                {
                    validPositions.Add(new Vector2Int(x, y));
                }
            }
        }

        return validPositions;
    }

    /// <summary>
    /// 基本的なオブジェクト配置可能性をチェック（ゲートのみ考慮）
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

                if (!IsValidMapPosition(checkX, checkY) || !IsGroundOrGrass(new Vector2Int(checkX, checkY)))
                {
                    return false; // 周囲に地面または草がない場合は配置不可
                }
            }
        }
        return mapBase[x, y] == (int)TileType.Ground || mapBase[x, y] == (int)TileType.Grass;
    }

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
    #endregion
}
