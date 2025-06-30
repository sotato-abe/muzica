using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FieldGenerator : MonoBehaviour
{
    [SerializeField] FieldData fieldData;
    [SerializeField] GameObject GateObject;
    public Tilemap tilemap;
    public string seed = "banana123"; // ここ変えたら別マップ
    private int[,] mapBase;
    private int[,] areaMapBase;
    int width = 30;
    int height = 20;
    private float groundFillPercent = 0.4f; // 埋め込む確率
    private float areaFillPercent = 0.2f; // 埋め込む確率
    public TileBase groundTile;
    public TileBase grassTile;
    public TileBase gateTile;
    public int objectCount = 5;
    private HashSet<Vector2Int> placedGates = new HashSet<Vector2Int>();
    public GameObject[] objectPrefabs;

    private Vector2Int topGate = new Vector2Int(0, 0);
    private Vector2Int bottomGate = new Vector2Int(0, 0);
    private Vector2Int leftGate = new Vector2Int(0, 0);
    private Vector2Int rightGate = new Vector2Int(0, 0);

    public void SetField(FieldData fieldData, FieldTileSet fieldTileSet, string seed = "banana123")
    {
        this.fieldData = fieldData;
        this.groundTile = fieldTileSet.GroundTile;
        this.grassTile = fieldTileSet.GrassTile;
        // this.objectPrefabs = fieldData.ObjectPrefabs;

        // フィールドのパラメータを設定
        width = fieldData.FieldWidth;
        height = fieldData.FieldHeight;
        groundFillPercent = fieldData.GroundFillPercent;
        areaFillPercent = fieldData.AreaFillPercent;
        objectCount = fieldData.ObjectCount;

        this.seed = seed;
        GenerateField();
    }

    public void GenerateField()
    {
        ClearField();
        // タイルマップをクリア
        Random.InitState(seed.GetHashCode());

        // グラウンドを生成
        mapBase = GenerateGroundMap(new int[width, height], groundFillPercent);
        areaMapBase = GenerateGroundMap(new int[width, height], areaFillPercent);

        mapBase = SmoothMap(mapBase);
        areaMapBase = SmoothMap(areaMapBase);

        // mapBaseにareaMapBaseをマージ（areaMapBaseが1のところをmapbaseに２で追加）
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (areaMapBase[x, y] == (int)TileType.Ground && mapBase[x, y] == (int)TileType.Ground)
                {
                    mapBase[x, y] = (int)TileType.Grass; // 地面とエリアの重なり
                }
            }
        }

        CreateAllGate();
        RenderingField();
        CreateObjects();
    }

    private int[,] GenerateGroundMap(int[,] draftMap, float fillPercent)
    {
        // シードで初期化して、モザイク状のフィールドマップをmapBaseに生成
        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                // ランダムに地面タイルを配置
                if (Random.value < fillPercent)
                {
                    draftMap[x + width / 2, y + height / 2] = (int)TileType.Ground; // 地面タイル
                }
                else
                {
                    draftMap[x + width / 2, y + height / 2] = (int)TileType.None; // 空白タイル
                }
            }
        }
        return draftMap;
    }

    // 　モザイク状のフィールドマップを滑らかにしていく
    private int[,] SmoothMap(int[,] draftMap)
    {
        for (int i = 0; i < 2; i++)
        {
            // 1回のスムージングで周囲の地面タイル数をカウントし、4以上なら地面、4未満なら空白にする
            for (int x = -width / 2; x < width / 2; x++)
            {
                for (int y = -height / 2; y < height / 2; y++)
                {
                    int gridX = x + width / 2;
                    int gridY = y + height / 2;

                    // 周囲の地面タイル数をカウント
                    int surroundingGroundCount = GetSurroundingGroundCount(gridX, gridY, draftMap);

                    // 4以上なら地面、4未満なら空白にする
                    if (surroundingGroundCount >= 4)
                    {
                        draftMap[gridX, gridY] = (int)TileType.Ground; // 地面タイル
                    }
                    else
                    {
                        draftMap[gridX, gridY] = (int)TileType.None; // 空白タイル
                    }
                }
            }
        }
        return draftMap;
    }

    //　指定座標の周囲のグラウンド数を取得
    private int GetSurroundingGroundCount(int gridX, int gridY, int[,] draftMap)
    {
        int count = 0;

        // 周囲の8方向をチェック
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                // 自分自身を除外
                if (x == 0 && y == 0) continue;

                int checkX = gridX + x;
                int checkY = gridY + y;

                // マップの範囲内であることを確認
                if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                {
                    count += draftMap[checkX, checkY];
                }
            }
        }

        return count;
    }

    private void CreateAllGate()
    {
        // 上端と下端にゲートを作成
        if (fieldData.IsTopOpen)
        {
            topGate = new Vector2Int(Random.Range(0, width), height - 1);
            CreateGate(topGate, Vector2Int.up);

        }
        if (fieldData.IsBottomOpen)
        {
            bottomGate = new Vector2Int(Random.Range(0, width), 0);
            CreateGate(bottomGate, Vector2Int.down);
        }
        if (fieldData.IsRightOpen)
        {
            rightGate = new Vector2Int(width - 1, Random.Range(0, height));
            CreateGate(rightGate, Vector2Int.right);
        }
        if (fieldData.IsLeftOpen)
        {
            leftGate = new Vector2Int(0, Random.Range(0, height));
            CreateGate(leftGate, Vector2Int.left);
        }
    }


    private void CreateGate(Vector2Int entry, Vector2Int direction)
    {
        mapBase[entry.x, entry.y] = (int)TileType.Gate;
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        int tileX = entry.x - width / 2;
        int tileY = entry.y - height / 2;
        Vector3Int tilePosition = new Vector3Int(tileX, tileY, 0);
        Vector3 worldPos = tilemap.GetCellCenterWorld(tilePosition) + new Vector3(0f, 0.25f, 0f);

        GameObject gateObj = Instantiate(GateObject, worldPos, Quaternion.identity);

        GateTrigger gateTrigger = gateObj.GetComponent<GateTrigger>();
        if (gateTrigger != null)
        {
            gateTrigger.direction = direction;
        }

        queue.Enqueue(entry);
        visited.Add(entry);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            if ((mapBase[current.x, current.y] == (int)TileType.Ground || mapBase[current.x, current.y] == (int)TileType.Grass) && !placedGates.Contains(current))
            {
                Vector2Int pathPos = current;
                while (cameFrom.ContainsKey(pathPos))
                {
                    mapBase[pathPos.x, pathPos.y] = (int)TileType.Ground;
                    pathPos = cameFrom[pathPos];
                }
                break;
            }

            Vector2Int[] directions = new Vector2Int[]
            {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
            };

            foreach (var dir in directions)
            {
                Vector2Int neighbor = current + dir;
                if (IsInMap(neighbor) && !visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }
    }

    private void CreateObjects()
    {
        int placed = 0;
        System.Random rand = new System.Random(seed.GetHashCode());

        while (placed < objectCount)
        {
            int x = rand.Next(0, width);
            int y = rand.Next(0, height);

            if (mapBase[x, y] == (int)TileType.Ground || mapBase[x, y] == (int)TileType.Grass)
            {
                Vector3 worldPos = tilemap.CellToWorld(new Vector3Int(x - width / 2, y - height / 2, 0)) + new Vector3(0.5f, 0.5f, 0f);

                Instantiate(objectPrefabs[rand.Next(objectPrefabs.Length)], worldPos, Quaternion.identity);
                placed++;
            }
        }
    }

    private bool IsInMap(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
    }

    private void RenderingField()
    {
        // タイルマップに地面タイルを配置
        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                int gridX = x + width / 2;
                int gridY = y + height / 2;

                if (mapBase[gridX, gridY] == (int)TileType.Ground)
                {
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);
                    tilemap.SetTile(tilePosition, groundTile);
                }
                else if (mapBase[gridX, gridY] == (int)TileType.Grass)
                {
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);
                    tilemap.SetTile(tilePosition, grassTile);
                }
                else if (mapBase[gridX, gridY] == (int)TileType.Gate)
                {
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);
                    tilemap.SetTile(tilePosition, groundTile);
                }
            }
        }
    }

    public void ClearField()
    {
        // タイルマップをクリア
        tilemap.ClearAllTiles();
        mapBase = new int[width, height];
        areaMapBase = new int[width, height];
        placedGates.Clear();
        foreach (var obj in GameObject.FindGameObjectsWithTag("FieldObject"))
        {
            Destroy(obj);
        }
        foreach (var obj in GameObject.FindGameObjectsWithTag("Gate"))
        {
            Destroy(obj);
        }
    }

    public Vector3Int GetEntorancePosition(Vector2Int direction)
    {
        Vector2Int targetPos = Vector2Int.zero;
        // ゲートの位置を取得して返す
        if (direction == Vector2Int.up && fieldData.IsTopOpen)
        {
            targetPos = bottomGate;
        }
        else if (direction == Vector2Int.down && fieldData.IsBottomOpen)
        {
            targetPos = topGate;
        }
        else if (direction == Vector2Int.right && fieldData.IsRightOpen)
        {
            targetPos = leftGate;
        }
        else if (direction == Vector2Int.left && fieldData.IsLeftOpen)
        {
            targetPos = rightGate;
        }
        else
        {
            targetPos = bottomGate;
        }

        Vector3Int tilePosition = new Vector3Int(targetPos.x - width / 2, targetPos.y - height / 2, 0);
        return tilePosition;
    }
}
