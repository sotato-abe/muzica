using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FieldDatabase : MonoBehaviour
{
    public static FieldDatabase Instance { get; private set; }
    [SerializeField] public FieldBase defaultFieldBase;
    public List<FieldBase> fieldBaseList;
    private Dictionary<Vector2Int, FieldBase> dataDict;

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
        dataDict = new Dictionary<Vector2Int, FieldBase>();

        foreach (var data in fieldBaseList)
        {
            if (data == null)
            {
                Debug.LogWarning("fieldBasebase: Null fieldBase found in list.");
                continue;
            }

            if (dataDict.ContainsKey(data.Position))
            {
                Debug.LogWarning($"fieldBasebase: Duplicate entry for {data.Position} found. Skipping.");
                continue;
            }

            dataDict[data.Position] = data;
        }
    }

    // targetPositionに基づいてpositionが一致するfieldBaseを返す
    public FieldBase GetFieldBaseByCoordinate(Vector2Int targetPosition)
    {
        foreach (var data in fieldBaseList)
        {
            if (data.Position == targetPosition)
            {
                return data;
            }
        }
        defaultFieldBase.currentPosition = targetPosition;

        return defaultFieldBase;
    }
}