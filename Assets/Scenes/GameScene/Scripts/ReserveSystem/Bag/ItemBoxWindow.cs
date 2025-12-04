using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ItemBoxWindow : MonoBehaviour, IDropHandler
{
    [SerializeField] ItemBlock itemBlockPrefab;
    [SerializeField] GameObject refusalBlockPrefab;
    [SerializeField] GameObject itemList;
    [SerializeField] GameObject blockList;
    [SerializeField] TextMeshProUGUI counterText;
    [SerializeField] int maxCount = 20;

    Dictionary<Item, ItemBlock> itemBlockMap = new Dictionary<Item, ItemBlock>();

    private void Awake()
    {
        DeleteAllItems();
    }
    private void OnEnable()
    {
        SetItems();
        SetBlock();
    }

    public void OnDrop(PointerEventData eventData)
    {
        // ドロップアイテムをバックに追加
        ItemBlock droppedItemBlock = eventData.pointerDrag?.GetComponent<ItemBlock>();
        if (droppedItemBlock == null || droppedItemBlock.Item == null) return;
        if (droppedItemBlock.OriginalParent == this.transform) return;

        Item item = droppedItemBlock.Item;
        bool canBuy = droppedItemBlock.RemoveItem();
        if (canBuy)
        {
            PlayerController.Instance.AddItemToBag(item);
            SetItems();
        }
    }

    public void SetItems()
    {
        List<Item> items = PlayerController.Instance.PlayerCharacter.BagItemList;

        // 現在のBagItemListにないアイテムブロックは削除する
        var itemsToRemove = new List<Item>();

        foreach (var kvp in itemBlockMap)
        {
            if (!items.Contains(kvp.Key))
            {
                itemsToRemove.Add(kvp.Key);
            }
        }

        foreach (var item in itemsToRemove)
        {
            if (itemBlockMap.TryGetValue(item, out ItemBlock itemBlock))
            {
                Destroy(itemBlock.gameObject);
                itemBlockMap.Remove(item);
            }
        }

        foreach (Item item in items)
        {
            if (itemBlockMap.ContainsKey(item))
            {
                itemBlockMap[item].SetStatusText();
                itemBlockMap[item].Show();
                continue;
            }
            CreateItemBlock(item);
        }

        SetCounter();
    }

    private void CreateItemBlock(Item item)
    {
        if (itemBlockMap.ContainsKey(item))
        {
            Debug.Log($"ItemBlock for {item.Base.Name} already exists, skipping creation.");
            return;
        }

        ItemBlock itemBlock = Instantiate(itemBlockPrefab, itemList.transform);
        itemBlock.Setup(item, this.transform);
        itemBlock.isOwned = true; // 所有フラグを設定
        itemBlock.OnRemoveItem += RemoveItem;
        itemBlockMap[item] = itemBlock;
    }

    private void SetCounter()
    {
        counterText.text = $"{itemBlockMap.Count} / {PlayerController.Instance.PlayerCharacter.Bag}";
    }

    private void SetBlock()
    {
        int newBlockCount = maxCount - PlayerController.Instance.PlayerCharacter.Bag;
        foreach (Transform child in blockList.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < newBlockCount; i++)
        {
            Instantiate(refusalBlockPrefab, blockList.transform);
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
        SetCounter();
    }

    private bool RemoveItem(ItemBlock itemBlock)
    {
        if (itemBlock == null || itemBlock.Item == null) return false;
        if (itemBlock.OriginalParent != this.transform) return false;

        Item item = itemBlock.Item;
        PlayerController.Instance.RemoveItemFromBag(item);
        itemBlock.RemovePlaceholder();
        itemBlockMap.Remove(item);
        Destroy(itemBlock.gameObject);
        SetItems();
        return true;
    }
}
