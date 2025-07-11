using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EquipmentSimpleWindow : MonoBehaviour, IDropHandler
{
    [SerializeField] ItemBlock itemBlockPrefab;
    [SerializeField] GameObject itemList;
    PlayerController playerController;
    Dictionary<Item, ItemBlock> itemBlockMap = new Dictionary<Item, ItemBlock>();

    public delegate void TargetItemDelegate(Item? item);
    public event TargetItemDelegate OnTargetItem;

    private int currentBlockCount = 0;
    private void Awake()
    {
        playerController = PlayerController.Instance;
        DeleteAllItems();
    }
    private void OnEnable()
    {
        if (PlayerController.Instance == null) return;

        playerController = PlayerController.Instance;
        SetItems();
    }

    public void OnDrop(PointerEventData eventData)
    {
        // ドロップアイテムをバックに追加
        ItemBlock droppedItemBlock = eventData.pointerDrag?.GetComponent<ItemBlock>();

        if (droppedItemBlock != null && droppedItemBlock.Item != null)
        {
            Item item = droppedItemBlock.Item;
            if (droppedItemBlock.OriginalParent == this.transform)
                return;
            if (item is Equipment || item is Treasure)
            {
                Debug.LogWarning("装備品や宝物はポケットにドロップできません。");
                return;
            }
            playerController.AddItemToEquip(droppedItemBlock.Item);
            droppedItemBlock.RemoveItem();
            SetItems();
        }
        else
        {
            Debug.LogWarning("ドロップされたアイテムが無効です。");
        }
    }

    public void SetItems()
    {
        List<Equipment> items = playerController.PlayerCharacter.EquipmentList;

        foreach (Item item in items)
        {
            if (itemBlockMap.ContainsKey(item))
            {
                // 既に表示済みならスキップ
                continue;
            }

            // 新規アイテムだけ生成
            ItemBlock itemBlock = Instantiate(itemBlockPrefab, itemList.transform);
            itemBlock.Setup(item, this.transform);
            itemBlock.OnRemoveItem += RemoveItem;
            itemBlock.OnTargetItem += TargetItem;
            itemBlockMap[item] = itemBlock;
        }

    }

    private void DeleteAllItems()
    {
        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var itemBlock in itemBlockMap.Values)
        {
            Destroy(itemBlock.gameObject);
        }
        itemBlockMap.Clear();
    }

    private void RemoveItem(ItemBlock itemBlock)
    {
        if (itemBlock == null || itemBlock.Item == null) return;

        Item item = itemBlock.Item;
        if (itemBlockMap.ContainsKey(item))
        {
            playerController.RemoveItemFromEquip(itemBlock.Item);
            itemBlock.RemovePlaceholder();
            itemBlockMap.Remove(item);
            Destroy(itemBlock.gameObject);
            SetItems();
        }
    }

    public void TargetItem(ItemBlock itemBlock)
    {
        if (itemBlock == null || itemBlock.Item == null)
        {
            OnTargetItem?.Invoke(null);
            return;
        }
        OnTargetItem?.Invoke(itemBlock.Item);
    }
}
