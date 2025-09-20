using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyQuestTask : MonoBehaviour
{
    [Header("Task")] // タスク
    [SerializeField] OrderItemSlot orderItemSlotPrefab;
    [SerializeField] GameObject supplyItemList;

    [Header("Reward")] // 報酬
    [SerializeField] MockItemBlock rewardItemPrefab;
    [SerializeField] GameObject rewardItemList;
    [SerializeField] CurrencyVal coinVal;
    [SerializeField] CurrencyVal discVal;

    private List<OrderItemSlot> orderItemSlots = new List<OrderItemSlot>();
    List<Item> orderItems = new List<Item>();

    public delegate void TargetItemDelegate(Item item);
    public event TargetItemDelegate OnTargetItem;

    public delegate void OwnerMessageDelegate(TalkMessage message);
    public event OwnerMessageDelegate OnOwnerMessage;

    public delegate void CompletedDelegate();
    public event CompletedDelegate OnCompleted;

    // ここにサプライクエストタスクのロジックを実装
    public void SetSupplyTask(SupplyQuest quest)
    {
        // サプライクエストのタスク設定ロジックをここに実装
        if (quest == null) return;
        ClearTask();
        foreach (var item in quest.OrderItems)
        {
            SetSupplyItemSlot(item);
        }
        foreach (var item in quest.RewardItems)
        {
            SetRewardItemSlot(item);
        }
        coinVal.SetCurrencyVal(quest.SupplyQuestBase.CoinPrice);
        discVal.SetCurrencyVal(quest.SupplyQuestBase.DiscPrice);
    }

    private void SetSupplyItemSlot(Item item)
    {
        var slot = Instantiate(orderItemSlotPrefab, supplyItemList.transform);
        slot.SetOrderItem(item);
        slot.OnTargetItem += TargetItem;
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
        slot.OnTargetItem += TargetItem;
    }

    private void ClearTask()
    {
        orderItemSlots.Clear();
        foreach (Transform child in supplyItemList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in rewardItemList.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void TargetItem(Item item)
    {
        OnTargetItem?.Invoke(item);
    }

    public void OwnerMessage(TalkMessage message)
    {
        OnOwnerMessage?.Invoke(message);
    }

    public List<Item> GetSupplyItems()
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