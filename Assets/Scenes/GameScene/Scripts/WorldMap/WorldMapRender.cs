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
        // OutputTileMapData();
        RenderGroundMap();
    }

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
                else
                {
                    Debug.LogWarning($"Tile not found for FieldType ID: {fieldTypeID} at position ({x}, {y})");
                }
            }
        }
    }

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
            return mapData;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"マップデータ読込エラー: {e.Message}");
            return null;
        }
    }

    private void RenderFieldMap()
    {
        //FieldDatabaseに登録されているFieldDataを取得
        // 各FieldDataのPositionにをfieldMapにIconをタイルにして配置
        foreach (var fieldData in FieldDatabase.Instance.fieldDataList)
        {
            if (fieldData == null)
            {
                Debug.LogWarning("FieldData is null, skipping.");
                continue;
            }

            Vector3Int cell = new Vector3Int(fieldData.Position.x, fieldData.Position.y, 0);
            if (fieldMap != null && fieldData.Icon != null)
            {
                Tile tile = ScriptableObject.CreateInstance<Tile>();
                tile.sprite = fieldData.Icon;
                fieldMap.SetTile(cell, tile);
            }
            else
            {
                Debug.LogWarning($"FieldMap or Icon is not assigned for position {fieldData.Position}");
            }
        }
    }
}