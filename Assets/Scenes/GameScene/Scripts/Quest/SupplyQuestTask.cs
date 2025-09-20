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

    public delegate void TargetItemDelegate(Item item);
    public event TargetItemDelegate OnTargetItem;

    public delegate void OwnerMessageDelegate(TalkMessage message);
    public event OwnerMessageDelegate OnOwnerMessage;

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
    }

    private void SetRewardItemSlot(Item item)
    {
        var slot = Instantiate(rewardItemPrefab, rewardItemList.transform);
        slot.SetMockItem(item);
        slot.OnTargetItem += TargetItem;
    }

    private void ClearTask()
    {
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
}