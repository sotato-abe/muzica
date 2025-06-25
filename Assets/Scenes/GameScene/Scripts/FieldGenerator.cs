using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FieldGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase groundTile;
    public TileBase gateTile;
    [SerializeField] FieldData fieldData;

    public string seed = "banana123"; // ここ変えたら別マップ
    private int[,] mapBase;
    int width = 30;
    int height = 20;
    private float fillPercent = 0.4f; // 埋め込む確率
    private float objectPercent = 0.2f; // 埋め込む確率
    private HashSet<Vector2Int> placedGates = new HashSet<Vector2Int>();

    void Start()
    {
        width = fieldData.FieldWidth;
        height = fieldData.FieldHeight;
        fillPercent = fieldData.FillPercent;
        groundTile = fieldData.GroundTile;
        gateTile = fieldData.GateTile;
        GenerateField();
    }

    void GenerateField()
    {
        // タイルマップをクリア
        tilemap.ClearAllTiles();
        Random.InitState(seed.GetHashCode());

        // グラウンドを生成
        GenerateGroundMap();

        for (int i = 0; i < 2; i++)
        {
            SmoothMap();
        }
        RenderingField();
        CreateAllGate();
    }

    void GenerateGroundMap()
    {
        // シードで初期化して、モザイク状のフィールドマップをmapBaseに生成
        mapBase = new int[width, height];
        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                // ランダムに地面タイルを配置
                if (Random.value < fillPercent)
                {
                    mapBase[x + width / 2, y + height / 2] = 1; // 地面タイル
                }
                else
                {
                    mapBase[x + width / 2, y + height / 2] = 0; // 空白タイル
                }
            }
        }
    }

    // 　モザイク状のフィールドマップを滑らかにしていく
    private void SmoothMap()
    {
        // 1回のスムージングで周囲の地面タイル数をカウントし、4以上なら地面、4未満なら空白にする
        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                int gridX = x + width / 2;
                int gridY = y + height / 2;

                // 周囲の地面タイル数をカウント
                int surroundingGroundCount = GetSurroundingGroundCount(gridX, gridY);

                // 4以上なら地面、4未満なら空白にする
                if (surroundingGroundCount >= 4)
                {
                    mapBase[gridX, gridY] = 1; // 地面タイル
                }
                else
                {
                    mapBase[gridX, gridY] = 0; // 空白タイル
                }
            }
        }
    }

    //　指定座標の周囲のグラウンド数を取得
    private int GetSurroundingGroundCount(int gridX, int gridY)
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
                    count += mapBase[checkX, checkY];
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
            CreateGate(new Vector2Int(Random.Range(0, width), height - 1));
        }
        if (fieldData.IsBottomOpen)
        {
            CreateGate(new Vector2Int(Random.Range(0, width), 0));
        }
        if (fieldData.IsRightOpen)
        {
            CreateGate(new Vector2Int(width - 1, Random.Range(0, height)));
        }
        if (fieldData.IsLeftOpen)
        {
            CreateGate(new Vector2Int(0, Random.Range(0, height)));
        }
        placedGates = new HashSet<Vector2Int>();
    }


    private void CreateGate(Vector2Int entry)
    {
        // Debug.Log($"Creating gate at {entry}");
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        queue.Enqueue(entry);
        visited.Add(entry);

        Vector2Int foundTarget = Vector2Int.zero;
        bool found = false;

        // 幅優先探索でGroundを探す
        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            // Groundを見つけたら終わり
            if (mapBase[current.x, current.y] == 1 && !placedGates.Contains(current))
            {
                placedGates.Add(current);
                found = true;
                Vector3Int tilePosition = new Vector3Int(current.x - width / 2, current.y - height / 2, 0);
                tilemap.SetTile(tilePosition, gateTile);
                break;
            }

            // 隣接セル（上下左右）をチェック
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

                if (mapBase[gridX, gridY] == 1)
                {
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);
                    tilemap.SetTile(tilePosition, groundTile);
                }
            }
        }
    }
}
