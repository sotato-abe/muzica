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

    public delegate void OwnerMessageDelegate(TalkMessage message);
    public event OwnerMessageDelegate OnOwnerMessage;

    private const int MAX_BAG_COUNT = 20;
    private Point currentPoint;

    public void OnDrop(PointerEventData eventData)
    {
        // Itemを販売
        ItemBlock droppedItemBlock = eventData.pointerDrag?.GetComponent<ItemBlock>();

        if (droppedItemBlock != null && droppedItemBlock.Item != null)
        {
            Item item = droppedItemBlock.Item;
            PlayerController.Instance.SellItem(item);
            droppedItemBlock.RemoveItem();
            string coinText = "C:" + ((item.Base.CoinPrice / 2).ToString() ?? "0");
            string discText = "D:" + ((item.Base.DiscPrice / 2).ToString() ?? "0");
            TalkMessage talkMessage = new TalkMessage(MessageType.Talk, MessagePanelType.Default, "買い取るよ");
            OnOwnerMessage?.Invoke(talkMessage);
            CreateItemBlock(item);
        }
        else
        {
            Debug.LogWarning("ドロップされたアイテムが無効です。");
        }
    }

    public void SetPoint(Point point)
    {
        currentPoint = point;
        SetItems(point.ShopItems);
    }

    public void SetItems(List<Item> items = null)
    {
        DeleteAllItems();
        foreach (Item item in items)
        {
            item.isNew = false;
            CreateItemBlock(item);
        }
    }

    private void CreateItemBlock(Item item)
    {
        ItemBlock itemBlock = Instantiate(itemBlockPrefab, itemList.transform);
        itemBlock.Setup(item, this.transform);
        itemBlock.OnRemoveItem += SellItem;
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
            currentPoint.ShopItems.Remove(item); // ポイントのアイテムリストから削除
            TalkMessage sellMessage = new TalkMessage(MessageType.Talk, MessagePanelType.Default, "ありがとうね");
            OnOwnerMessage?.Invoke(sellMessage);
            return true;
        }
        TalkMessage talkMessage = new TalkMessage(MessageType.Talk, MessagePanelType.Default, "お金が足りないよ");
        OnOwnerMessage?.Invoke(talkMessage);
        return false;
    }
}
