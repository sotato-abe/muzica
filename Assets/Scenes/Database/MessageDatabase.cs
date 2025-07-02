using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class MessageDatabase : MonoBehaviour
{
    public static MessageDatabase Instance { get; private set; }

    public List<MessageData> messageDataList;
    private Dictionary<MessageIconType, MessageData> dataDict;

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
        dataDict = new Dictionary<MessageIconType, MessageData>();

        foreach (var data in messageDataList)
        {
            if (data == null)
            {
                Debug.LogWarning("MessageDatabase: Null MessageData found in list.");
                continue;
            }

            if (dataDict.ContainsKey(data.type))
            {
                Debug.LogWarning($"MessageDatabase: Duplicate entry for {data.type} found. Skipping.");
                continue;
            }

            dataDict[data.type] = data;
        }
    }

    public MessageData GetData(MessageIconType type)
    {
        if (dataDict != null && dataDict.TryGetValue(type, out var data))
        {
            return data;
        }

        return null;
    }

    public Sprite GetIcon(MessageIconType type)
    {
        if (dataDict != null && dataDict.TryGetValue(type, out var data))
        {
            return data.icon;
        }

        return null;
    }
}