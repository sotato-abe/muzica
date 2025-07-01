using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Tilemaps;

public class FieldTileSetDatabase : MonoBehaviour
{
    public static FieldTileSetDatabase Instance { get; private set; }

    public List<FieldTileSet> FieldTileSetList;
    private Dictionary<FieldType, FieldTileSet> dataDict;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Initialize();
    }

    private void Initialize()
    {
        dataDict = new Dictionary<FieldType, FieldTileSet>();

        foreach (var data in FieldTileSetList)
        {
            if (data == null)
            {
                Debug.LogWarning("FieldTileSetDatabase: Null FieldTileSet found in list.");
                continue;
            }

            if (dataDict.ContainsKey(data.FieldType))
            {
                Debug.LogWarning($"FieldTileSetDatabase: Duplicate entry for {data.FieldType} found. Skipping.");
                continue;
            }

            dataDict[data.FieldType] = data;
        }
    }

    // FieldTypeに基づいてFieldTileSetを返す
    public FieldTileSet GetTileSetFromByType(FieldType type)
    {
        if (dataDict != null && dataDict.TryGetValue(type, out var data))
        {
            return data;
        }

        return null;
    }

    public FieldTileSet GetTileSetFromByTile(TileBase tile)
    {
        if (tile == null)
        {
            Debug.LogWarning("Tile is null, cannot convert to FieldType.");
            return null;
        }

        FieldType fieldType = FieldType.Default;

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
                Debug.LogWarning($"Unknown tile type: {tile.name}");
                break;
        }
        return GetTileSetFromByType(fieldType);
    }

    public TileBase GetGroundTileByType(FieldType type)
    {
        FieldTileSet tileSet = GetTileSetFromByType(type);
        if (tileSet != null)
        {
            return tileSet.GroundTile;
        }
        if (type != FieldType.None)
        {
            Debug.LogWarning($"No GroundTile found for FieldType: {type}");
        }
        return null;
    }
}