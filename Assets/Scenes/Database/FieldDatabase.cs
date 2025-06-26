using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FieldDatabase : MonoBehaviour
{
    public static FieldDatabase Instance { get; private set; }

    public List<FieldData> fieldDataList;
    private Dictionary<FieldType, FieldData> dataDict;

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
        dataDict = new Dictionary<FieldType, FieldData>();

        foreach (var data in fieldDataList)
        {
            if (data == null)
            {
                Debug.LogWarning("fieldDatabase: Null fieldData found in list.");
                continue;
            }

            if (dataDict.ContainsKey(data.FieldTileSet.FieldType))
            {
                Debug.LogWarning($"fieldDatabase: Duplicate entry for {data.FieldTileSet.FieldType} found. Skipping.");
                continue;
            }

            dataDict[data.FieldTileSet.FieldType] = data;
        }
    }

    // FieldTypeに基づいてfieldDataを返す
    public FieldData GetData(FieldType type)
    {
        if (dataDict != null && dataDict.TryGetValue(type, out var data))
        {
            return data;
        }

        return null;
    }
}