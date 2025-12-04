using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryQuestTask : MonoBehaviour
{
    [SerializeField] OrderItemSlot orderItemSlotPrefab;
    [SerializeField] MockItemBlock rewardItemPrefab;
    [SerializeField] GameObject deliveryItemList;
    [SerializeField] GameObject rewardItemList;
    [SerializeField] CurrencyVal coinVal;
    [SerializeField] CurrencyVal discVal;

    private List<OrderItemSlot> orderItemSlots = new List<OrderItemSlot>();
    List<Item> orderItems = new List<Item>();

    public delegate void OwnerMessageDelegate(TalkMessage message);
    public event OwnerMessageDelegate OnOwnerMessage;

    public delegate void CompletedDelegate();
    public event CompletedDelegate OnCompleted;

    // ここにサプライクエストタスクのロジックを実装
    public void SetDeliveryTask(DeliveryQuest quest)
    {
        // サプライクエストのタスク設定ロジックをここに実装
        if (quest == null) return;
        ClearTask();
        foreach (var item in quest.DeliveryItemList)
        {
            SetDeliveryItemSlot(item);
        }
        foreach (var item in quest.RewardItems)
        {
            SetRewardItemSlot(item);
        }
        coinVal.SetCurrencyVal(quest.DeliveryQuestBase.CoinPrice);
        discVal.SetCurrencyVal(quest.DeliveryQuestBase.DiscPrice);
    }

    private void SetDeliveryItemSlot(Item item)
    {
        var slot = Instantiate(orderItemSlotPrefab, deliveryItemList.transform);
        slot.SetOrderItem(item);
        slot.OnOwnerMessage += OwnerMessage;
        slot.OnSetItem += CheckAllOrderItemSet;
        orderItemSlots.Add(slot);
    }

    public void CheckAllOrderItemSet()
    {
        foreach (var slot in orderItemSlots)
        {
            if (slot == null || !slot.IsSet)
            {
                OwnerMessage(new TalkMessage(MessageType.Other, MessagePanelType.Default, "まだ足りんのう"));
                return;
            }
        }

        OwnerMessage(new TalkMessage(MessageType.Other, MessagePanelType.Surprise, "これもらっていいんか？"));
        OnCompleted?.Invoke();
    }

    private void SetRewardItemSlot(Item item)
    {
        var slot = Instantiate(rewardItemPrefab, rewardItemList.transform);
        slot.SetMockItem(item);
    }

    private void ClearTask()
    {
        orderItemSlots.Clear();
        foreach (Transform child in deliveryItemList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in rewardItemList.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void OwnerMessage(TalkMessage message)
    {
        OnOwnerMessage?.Invoke(message);
    }

    public List<Item> GetDeliveryItems()
    {
        List<Item> items = new List<Item>();
        foreach (var slot in orderItemSlots)
        {
            if (slot != null && slot.IsSet)
            {
                items.Add(slot.currentItem);
            }
        }
        return items;
    }
}