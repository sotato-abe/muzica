using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FieldDatabase : MonoBehaviour
{
    public static FieldDatabase Instance { get; private set; }

    [SerializeField] private FieldBase defaultFieldBase;
    [SerializeField] private List<FieldBase> callistFieldBaseList;
    [SerializeField] private List<FieldBase> dokokuFieldBaseList;
    [SerializeField] private List<FieldBase> liburutoFieldBaseList;
    [SerializeField] private List<FieldBase> parteaFieldBaseList;
    [SerializeField] private List<FieldBase> inviolableFieldBaseList;
    private List<FieldBase> fieldBaseList = new List<FieldBase>();
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

        fieldBaseList.AddRange(callistFieldBaseList);
        fieldBaseList.AddRange(dokokuFieldBaseList);
        fieldBaseList.AddRange(liburutoFieldBaseList);
        fieldBaseList.AddRange(parteaFieldBaseList);
        fieldBaseList.AddRange(inviolableFieldBaseList);

        foreach (var data in fieldBaseList)
        {
            if (data == null)
            {
                Debug.LogWarning("fieldBasebase: Null fieldBase found in list.");
                continue;
            }

            if (dataDict.ContainsKey(data.Position))
            {
                Debug.LogWarning($"fieldBasebase: Duplicate entry for {data.Position} / {data.FieldName} found. Skipping. same position already exists for {dataDict[data.Position].FieldName}.");
                continue;
            }

            dataDict[data.Position] = data;
        }
    }

    // targetPositionに基づいてpositionが一致するfieldBaseを返す
    public FieldBase GetFieldBaseByCoordinate(Vector2Int targetPosition)
    {
        if (dataDict.TryGetValue(targetPosition, out FieldBase fieldBase))
        {
            return fieldBase;
        }
        defaultFieldBase.currentPosition = targetPosition;

        return defaultFieldBase;
    }

    public List<FieldBase> GetAllFieldBases()
    {
        return new List<FieldBase>(fieldBaseList);
    }
}