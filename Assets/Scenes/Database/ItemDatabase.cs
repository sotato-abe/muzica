using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance { get; private set; }
    [SerializeField] List<ConsumableBase> consumableDataList;
    [SerializeField] List<EquipmentBase> equipmentDataList;
    [SerializeField] List<TreasureBase> treasureDataList;
    List<ItemBase> itemDataList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン切り替えても残す
        }
        else
        {
            Destroy(gameObject); // 重複防止
        }

        itemDataList = new List<ItemBase>();
        itemDataList.AddRange(consumableDataList);
        itemDataList.AddRange(equipmentDataList);
        itemDataList.AddRange(treasureDataList);
    }

    public ItemBase LoadItemData(int itemId)
    {

        if (itemId < 0 || itemId >= itemDataList.Count)
        {
            Debug.LogError("Invalid item ID: " + itemId);
            return null;
        }

        ItemBase item = itemDataList[itemId];
        return item;
    }

    public Item GetItemFromId(int itemId)
    {
        if (itemId < 0 || itemId >= itemDataList.Count)
        {
            Debug.LogWarning("Invalid item ID: " + itemId);
            return null;
        }

        ItemBase baseData = itemDataList[itemId];
        switch (baseData.itemType)
        {
            case ItemType.Consumable:
                return new Consumable((ConsumableBase)baseData, false);
            case ItemType.Equipment:
                return new Equipment((EquipmentBase)baseData, false);
            case ItemType.Treasure:
                return new Treasure((TreasureBase)baseData, false);
            default:
                Debug.LogError("Unknown item type: " + baseData.itemType);
                return null;
        }
    }

    public int GetItemId(ItemBase item)
    {
        if (item == null)
        {
            return -1;
        }
        return itemDataList.IndexOf(item);
    }

    public List<EquipmentBase> GetAllEquipmentBases()
    {
        return new List<EquipmentBase>(equipmentDataList);
    }

    public List<ConsumableBase> GetAllConsumableBases()
    {
        return new List<ConsumableBase>(consumableDataList);
    }

    public List<TreasureBase> GetAllTreasureBases()
    {
        return new List<TreasureBase>(treasureDataList);
    }
}