using UnityEngine;
using TMPro;

public class WorkQuestTask : MonoBehaviour
{
    [Header("Reward")] // 報酬
    [SerializeField] public TextMeshProUGUI monthText;
    [SerializeField] MockItemBlock rewardItemPrefab;
    [SerializeField] GameObject rewardItemList;
    [SerializeField] CurrencyVal coinVal;
    [SerializeField] CurrencyVal discVal;

    public delegate void TargetItemDelegate(Item item);
    public event TargetItemDelegate OnTargetItem;

    public delegate void OwnerMessageDelegate(TalkMessage message);
    public event OwnerMessageDelegate OnOwnerMessage;

    public delegate void CompletedDelegate();
    public event CompletedDelegate OnCompleted;

    public void SetTask(WorkQuest quest)
    {
        // サプライクエストのタスク設定ロジックをここに実装
        if (quest == null) return;
        ClearTask();
        monthText.text = quest.WorkQuestBase.Month.ToString() + "ヶ月";
        foreach (var item in quest.RewardItems)
        {
            SetRewardItemSlot(item);
        }
        coinVal.SetCurrencyVal(quest.WorkQuestBase.CoinPrice);
        discVal.SetCurrencyVal(quest.WorkQuestBase.DiscPrice);
    }

    private void SetRewardItemSlot(Item item)
    {
        var slot = Instantiate(rewardItemPrefab, rewardItemList.transform);
        slot.SetMockItem(item);
        slot.OnTargetItem += TargetItem;
    }

    private void ClearTask()
    {
        monthText.text = "";
        foreach (Transform child in rewardItemList.transform)
        {
            Destroy(child.gameObject);
        }
        coinVal.SetCurrencyVal(0);
        discVal.SetCurrencyVal(0);
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