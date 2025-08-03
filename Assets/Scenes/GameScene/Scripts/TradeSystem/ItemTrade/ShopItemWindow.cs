using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ShopItemWindow : MonoBehaviour, IDropHandler
{
    [SerializeField] ItemBlock itemBlockPrefab;
    [SerializeField] GameObject itemList;

    public delegate void TargetItemDelegate(Item? item, bool isOwn = false);
    public event TargetItemDelegate OnTargetItem;

    private const int MAX_BAG_COUNT = 20;
    private int currentBlockCount = 0;

    public void OnDrop(PointerEventData eventData)
    {
        // Itemを販売
        ItemBlock droppedItemBlock = eventData.pointerDrag?.GetComponent<ItemBlock>();

        if (droppedItemBlock != null && droppedItemBlock.Item != null)
        {
            Item item = droppedItemBlock.Item;
            PlayerController.Instance.SellItem(item);
            droppedItemBlock.RemoveItem();
            CreateItemBlock(item, null);
        }
        else
        {
            Debug.LogWarning("ドロップされたアイテムが無効です。");
        }
    }

    public void SetItems(List<Item> items = null)
    {
        DeleteAllItems();
        foreach (Item item in items)
        {
            CreateItemBlock(item, null);
        }
    }

    private void CreateItemBlock(Item item, string? statusText)
    {
        ItemBlock itemBlock = Instantiate(itemBlockPrefab, itemList.transform);
        itemBlock.Setup(item, this.transform);
        itemBlock.SetStatustext(statusText);
        itemBlock.OnRemoveItem += SellItem;
        itemBlock.OnTargetItem += TargetItem;
    }

    private void DeleteAllItems()
    {
        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private bool SellItem(ItemBlock itemBlock)
    {
        if (itemBlock == null || itemBlock.Item == null) return false;
        if (itemBlock.OriginalParent != this.transform) return false;

        Item item = itemBlock.Item;
        bool isBuy = PlayerController.Instance.SpendCurrency(item.Base.CoinPrice, item.Base.DiscPrice);
        if (isBuy)
        {
            itemBlock.RemovePlaceholder();
            Destroy(itemBlock.gameObject);
            return true;
        }
        return false;
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
