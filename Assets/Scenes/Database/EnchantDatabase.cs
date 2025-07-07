using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class EnchantDatabase : MonoBehaviour
{
    public static EnchantDatabase Instance { get; private set; }

    public List<EnchantData> enchantDataList;
    private Dictionary<EnchantType, EnchantData> dataDict;

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
        dataDict = new Dictionary<EnchantType, EnchantData>();

        foreach (var data in enchantDataList)
        {
            if (data == null)
            {
                Debug.LogWarning("EnchantDatabase: Null EnchantData found in list.");
                continue;
            }

            if (dataDict.ContainsKey(data.enchantType))
            {
                Debug.LogWarning($"EnchantDatabase: Duplicate entry for {data.enchantType} found. Skipping.");
                continue;
            }

            dataDict[data.enchantType] = data;
        }
    }

    public EnchantData GetData(EnchantType type)
    {
        if (dataDict != null && dataDict.TryGetValue(type, out var data))
        {
            return data;
        }

        return null;
    }
}