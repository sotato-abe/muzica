using UnityEngine;
using UnityEngine.Tilemaps;

public class FieldGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase groundTile;
    [SerializeField] FieldData fiieldData;

    public string seed = "banana123"; // ここ変えたら別マップ
    private int[,] mapBase;
    int width = 30;
    int height = 20;
    private float fillPercent = 0.4f; // 埋め込む確率
    private float objectPercent = 0.2f; // 埋め込む確率

    void Start()
    {
        width = fiieldData.FieldWidth;
        height = fiieldData.FieldHeight;
        fillPercent = fiieldData.FillPercent;
        groundTile = fiieldData.GroundTile;
        GenerateField();
    }

    void GenerateField()
    {
        Debug.Log("GenerateField");

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
    }

    void GenerateGroundMap()
    {
        Debug.Log("GenerateGround");
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
        Debug.Log("SmoothMap");

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

    private void RenderingField()
    {
        Debug.Log("RenderingField");

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
