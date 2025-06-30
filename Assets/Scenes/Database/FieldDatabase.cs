using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FieldDatabase : MonoBehaviour
{
    public static FieldDatabase Instance { get; private set; }

    public List<FieldData> fieldDataList;
    private Dictionary<Vector2Int, FieldData> dataDict;

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
        dataDict = new Dictionary<Vector2Int, FieldData>();

        foreach (var data in fieldDataList)
        {
            if (data == null)
            {
                Debug.LogWarning("fieldDatabase: Null fieldData found in list.");
                continue;
            }

            if (dataDict.ContainsKey(data.Position))
            {
                Debug.LogWarning($"fieldDatabase: Duplicate entry for {data.Position} found. Skipping.");
                continue;
            }

            dataDict[data.Position] = data;
        }
    }

    // targetPositionに基づいてpositionが一致するfieldDataを返す
    public FieldData GetFieldDataByCoordinate(Vector2Int targetPosition)
    {
        foreach (var data in fieldDataList)
        {
            if (data.Position == targetPosition)
            {
                return data;
            }
        }

        return null; // nullを返す
    }
}