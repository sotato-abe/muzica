using UnityEngine;

public class SupplyQuestTask : MonoBehaviour
{
    [Header("Task")] // タスク
    [SerializeField] OrderItemSlot orderItemSlotPrefab;
    [SerializeField] GameObject orderItemList;

    [Header("Reward")] // 報酬
    [SerializeField] MockItemBlock rewardItemPrefab;
    [SerializeField] GameObject supplyItemList;
    [SerializeField] GameObject rewardItemList;
    [SerializeField] CurrencyVal coinVal;
    [SerializeField] CurrencyVal discVal;

    // ここにサプライクエストタスクのロジックを実装
    public void SetSupplyTask(SupplyQuest quest)
    {
        // サプライクエストのタスク設定ロジックをここに実装
        if (quest == null) return;
        ClearTask();
        foreach (var item in quest.SupplyQuestBase.SupplyItemBaseList)
        {
            SetSupplyItemSlot(item);
        }
        foreach (var item in quest.SupplyQuestBase.RewardItemList)
        {
            SetRewardItemSlot(item);
        }
        coinVal.SetCurrencyVal(quest.SupplyQuestBase.CoinPrice);
        discVal.SetCurrencyVal(quest.SupplyQuestBase.DiscPrice);
    }

    private void SetSupplyItemSlot(ItemBase item)
    {
        var slot = Instantiate(orderItemSlotPrefab, supplyItemList.transform);
        slot.SetOrderItem(item);
    }

    private void SetRewardItemSlot(Item item)
    {
        var slot = Instantiate(rewardItemPrefab, rewardItemList.transform);
        slot.SetMockItem(item);
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
}