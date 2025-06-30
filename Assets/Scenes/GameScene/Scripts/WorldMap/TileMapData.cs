using System.Collections.Generic;

[System.Serializable]
public class TileMapData
{
    public List<int[]> data; // マップデータ
    public int rows;         // 行数
    public int cols;         // 列数

    public TileMapData(List<int[]> listData)
    {
        data = listData ?? new List<int[]>();
        rows = data.Count;
        cols = rows > 0 ? data[0].Length : 0;
    }

    // デフォルトコンストラクタ（必要）
    public TileMapData()
    {
        data = new List<int[]>();
        rows = 0;
        cols = 0;
    }
}
