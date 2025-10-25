using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using Newtonsoft.Json;

public class WorldMapRender : MonoBehaviour
{
    [SerializeField] private Tilemap groundMap; // 対象のTilemap
    [SerializeField] public Tilemap fieldMap;
    private int[,] groundMapBase;

    private void Start()
    {
        // OutputTileMapData(); // JSONファイルを出力する場合はコメントアウトを外す
        RenderGroundMap();
        RenderFieldMap();
    }

    /// <summary>
    /// タイルマップのデータをJSON形式で読み込み、groundMapにタイルを配置
    /// </summary>
    private void RenderGroundMap()
    {
        TileMapData mapData = LoadJsonMapData("GroundMapData");

        groundMap.ClearAllTiles(); // 既存のタイルをクリア
        for (int y = 0; y < mapData.rows; y++)
        {
            for (int x = 0; x < mapData.cols; x++)
            {
                int fieldTypeID = mapData.data[y][x];
                TileBase tile = FieldTileSetDatabase.Instance.GetGroundTileByType((FieldType)fieldTypeID);
                if (tile != null)
                {
                    Vector3Int position = new Vector3Int(x, y, 0);
                    groundMap.SetTile(position, tile);
                }
            }
        }
    }

    /// <summary>
    /// JSONファイルからTileMapDataを読み込む
    /// </summary>
    private TileMapData LoadJsonMapData(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName + ".json");

        if (!File.Exists(filePath))
        {
            Debug.LogError($"JSONファイルが見つかりません: {filePath}");
            return null;
        }

        try
        {
            string jsonData = File.ReadAllText(filePath);
            TileMapData mapData = JsonConvert.DeserializeObject<TileMapData>(jsonData);
            // groundMapBase にjsonDataのデータを設定
            groundMapBase = new int[mapData.rows, mapData.cols];
            for (int y = 0; y < mapData.rows; y++)
            {
                for (int x = 0; x < mapData.cols; x++)
                {
                    groundMapBase[y, x] = mapData.data[y][x];
                }
            }
            return mapData;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"マップデータ読込エラー: {e.Message}");
            return null;
        }
    }
    /// <summary>
    /// FieldBasebaseに登録されているFieldBaseを取得
    /// 各FieldBaseのPositionにをfieldMapにIconをタイルにして配置
    /// </summary>
    private void RenderFieldMap()
    {
        foreach (var fieldBase in FieldDatabase.Instance.FieldBaseList)
        {
            if (fieldMap != null && fieldBase.Icon != null)
            {
                Vector3Int cell = new Vector3Int(fieldBase.Position.x, fieldBase.Position.y, 0);
                Tile tile = ScriptableObject.CreateInstance<Tile>();
                tile.sprite = fieldBase.Icon;
                fieldMap.SetTile(cell, tile);

                fieldMap.SetTransformMatrix(cell, Matrix4x4.TRS(new Vector3(0, 0.15f, 0), Quaternion.identity, Vector3.one));
            }
        }
    }

    /// <summary>
    /// FieldMapのタイルマップデータをJSON形式で出力
    /// </summary>
    public void OutputTileMapData()
    {
        if (groundMap == null)
        {
            Debug.LogError("Tilemapが設定されていません。");
            return;
        }

        // Tilemapの範囲を取得
        groundMap.CompressBounds();
        BoundsInt bounds = groundMap.cellBounds;

        // 1次元配列のリストを用意（フラットな形式）
        List<int[]> tileData = new List<int[]>();
        int rows = bounds.size.y;
        int cols = bounds.size.x;

        for (int y = bounds.yMin; y < bounds.yMax; y++)
        {
            int[] row = new int[cols];  // 行データを保持する配列
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                bool hasTile = groundMap.HasTile(position); // タイルの有無をチェック
                FieldType fieldType = FieldType.None;
                if (hasTile)
                {
                    TileBase tile = groundMap.GetTile<TileBase>(position);
                    Debug.Log(tile.name);
                    switch (tile.name)
                    {
                        case string name when name.Contains("Default"):
                            fieldType = FieldType.Default;
                            break;
                        case string name when name.Contains("Wilderness"):
                            fieldType = FieldType.Wilderness;
                            break;
                        case string name when name.Contains("Grasslands"):
                            fieldType = FieldType.Grasslands;
                            break;
                        case string name when name.Contains("Sea"):
                            fieldType = FieldType.Sea;
                            break;
                        case string name when name.Contains("Desert"):
                            fieldType = FieldType.Desert;
                            break;
                        case string name when name.Contains("Wetlands"):
                            fieldType = FieldType.Wetlands;
                            break;
                        case string name when name.Contains("Snow"):
                            fieldType = FieldType.Snow; // 未実装  
                            break;
                        case string name when name.Contains("Rock"):
                            fieldType = FieldType.Rock;
                            break;
                        case string name when name.Contains("Magma"):
                            fieldType = FieldType.Magma; // 未実装
                            break;  
                        case string name when name.Contains("Pollution"):
                            fieldType = FieldType.Pollution; // 未実装
                            break;
                        case string name when name.Contains("Ocean"):
                            fieldType = FieldType.Ocean;
                            break; 
                        default:
                            fieldType = FieldType.None;
                            break;
                    }
                }
                row[x - bounds.xMin] = (int)fieldType;  // タイルがあれば1、なければ0を追加
            }
            tileData.Add(row);  // 1行分を追加
        }

        // TileMapDataを作成
        TileMapData tileMapData = new TileMapData(tileData); // フラットな配列リストを渡す

        // JSONに変換して出力（Newtonsoft.Jsonを使用）
        string jsonData = JsonConvert.SerializeObject(tileMapData, Formatting.Indented); // Newtonsoft.Jsonを使用

        // JSONをファイルに保存
        string filePath = Path.Combine(Application.persistentDataPath, "GroundMapData.json");
        try
        {
            File.WriteAllText(filePath, jsonData);
            Debug.Log($"ファイルに書き込みました: {filePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"ファイル書き込みに失敗しました: {e.Message}");
        }

        Debug.Log($"タイルマップデータをJSONとして出力しました: {filePath}");
    }

    //groundMapBaseの位置にFieldMapがあるかどうかを確認するメソッド
    public bool HasFieldMap(Vector2Int position)
    {
        if (groundMapBase == null)
        {
            Debug.LogError("groundMapBaseが初期化されていません。");
            return false;
        }

        // 範囲外チェック
        if (position.x < 0 || position.y < 0 || position.y >= groundMapBase.GetLength(0) || position.x >= groundMapBase.GetLength(1))
        {
            Debug.LogWarning($"座標 {position} は範囲外です。");
            return false;
        }

        // 指定位置にFieldMapがあるかどうかを確認
        return groundMapBase[position.y, position.x] != (int)FieldType.None 
            && groundMapBase[position.y, position.x] != (int)FieldType.Ocean;
    }
}