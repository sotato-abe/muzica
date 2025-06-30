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
    private const float GATE_OBJECT_Y_OFFSET = 0.1f;
    private const float OBJECT_POSITION_OFFSET = 0.5f;

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
    [SerializeField] private Tilemap tilemap;
    private string seed;

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

    // ゲート位置
    private readonly Dictionary<Vector2Int, Vector2Int> gatePositions = new Dictionary<Vector2Int, Vector2Int>();
    #endregion

    #region Public Properties
    public Tilemap Tilemap => tilemap;
    public FieldData fieldData;
    #endregion

    #region Public Methods
    /// <summary>
    /// フィールドを設定して生成する
    /// </summary>
    /// <param name="fieldData">フィールドデータ</param>
    /// <param name="fieldTileSet">タイルセット</param>
    /// <param name="seed">生成シード</param>
    public void SetField(FieldData fieldData, FieldTileSet fieldTileSet, string seed)
    {
        if (fieldTileSet == null)
        {
            Debug.LogError("FieldTileSet is null!");
            return;
        }

        InitializeField(fieldData, fieldTileSet, seed);
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
        InitializeRandomSeed();
        GenerateTerrainMaps();
        ProcessTerrainMaps();
        CreateAllGates();
        RenderField();
        CreateFieldObjects();
    }

    /// <summary>
    /// プレイヤーの入場位置を取得する
    /// </summary>
    /// <param name="direction">移動方向</param>
    /// <returns>入場位置のタイル座標</returns>
    public Vector3Int GetEntrancePosition(Vector2Int direction)
    {
        Vector2Int targetPos = GetGatePosition(direction);
        // targetPosに進行方向の反対方向を加える
        targetPos -= GetOppositeDirection(direction);
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
    private void InitializeField(FieldData fieldData, FieldTileSet fieldTileSet, string seed)
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

        this.seed = seed;
    }

    /// <summary>
    /// ランダムシードを初期化
    /// </summary>
    private void InitializeRandomSeed()
    {
        Random.InitState(seed.GetHashCode());
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
                }else
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
        if (fieldData.IsTopOpen)
        {
            Vector2Int topGate = new Vector2Int(Random.Range(width / 4, width * 3 / 4), height - 1);
            CreateGate(topGate, Vector2Int.up);
            gatePositions[Vector2Int.up] = topGate;
        }

        if (fieldData.IsBottomOpen)
        {
            Vector2Int bottomGate = new Vector2Int(Random.Range(width / 4, width * 3 / 4), 0);
            CreateGate(bottomGate, Vector2Int.down);
            gatePositions[Vector2Int.down] = bottomGate;
        }

        if (fieldData.IsRightOpen)
        {
            Vector2Int rightGate = new Vector2Int(width - 1, Random.Range(height / 4, height * 3 / 4));
            CreateGate(rightGate, Vector2Int.right);
            gatePositions[Vector2Int.right] = rightGate;
        }

        if (fieldData.IsLeftOpen)
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
        GameObject gateObj = Instantiate(gateObject, worldPos, Quaternion.identity);

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

        int placed = 0;
        System.Random rand = new System.Random(seed.GetHashCode());

        while (placed < objectCount)
        {
            int x = rand.Next(0, width);
            int y = rand.Next(0, height);

            if (CanPlaceObject(x, y))
            {
                Vector3 worldPos = GetObjectWorldPosition(x, y);
                InstantiateRandomObject(worldPos, rand);
                placed++;
            }
        }
    }

    /// <summary>
    /// オブジェクトを配置可能かチェック
    /// </summary>
    private bool CanPlaceObject(int x, int y)
    {
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
    /// ランダムなオブジェクトをインスタンス化
    /// </summary>
    private void InstantiateRandomObject(Vector3 position, System.Random rand)
    {
        GameObject prefab = objectPrefabs[rand.Next(objectPrefabs.Length)];
        if (prefab != null)
        {
            Instantiate(prefab, position, Quaternion.identity);
        }
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
    #endregion
}
