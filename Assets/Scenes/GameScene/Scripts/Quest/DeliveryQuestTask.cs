using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeliveryQuestTask : MonoBehaviour
{
    [SerializeField] Image characterImage;
    [SerializeField] Image characterFrame;
    [SerializeField] TextMeshProUGUI nameTitle;
    [SerializeField] TextMeshProUGUI addressTitle;

    [SerializeField] OrderItemSlot deliveryItemSlotPrefab;
    [SerializeField] RewardItemSlot rewardItemSlotPrefab;
    [SerializeField] GameObject deliveryItemList;
    [SerializeField] GameObject rewardItemList;

    [SerializeField] CurrencyVal coinVal;
    [SerializeField] CurrencyVal discVal;
    // ここにデリバリークエストタスクのロジックを実装
    public void SetDeliveryTask(DeliveryQuest quest)
    {
        if (quest == null) return;
        ClearTask();
        // サプライクエストのタスク設定ロジックをここに実装
        SetDeliveryCharacter(quest);
        foreach (var item in quest.DeliveryQuestBase.DeliveryItemBaseList)
        {
            SetDeliveryItemSlot(item);
        }
        foreach (var item in quest.DeliveryQuestBase.RewardItemList)
        {
            SetRewardItemSlot(item);
        }
        coinVal.SetCurrencyVal(quest.DeliveryQuestBase.CoinPrice);
        discVal.SetCurrencyVal(quest.DeliveryQuestBase.DiscPrice);
    }

    private void SetDeliveryCharacter(DeliveryQuest quest)
    {
        if (quest == null) return;
        var character = quest.DeliveryQuestBase.DeliveryCharacter;
        if (character == null) return;
        characterImage.sprite = character.SquareSprite;
        Color frameColor = character.Rarity.GetRarityColor();
        characterFrame.color = frameColor;
        nameTitle.text = character.Name;

        string raw = quest.DeliveryQuestBase.Address;
        string address = string.IsNullOrWhiteSpace(raw) ? "不明" : raw;

        addressTitle.text = address;
    }

    private void SetDeliveryItemSlot(TreasureBase item)
    {
        var slot = Instantiate(deliveryItemSlotPrefab, deliveryItemList.transform);
        slot.SetOrderItem(item);
        slot.OnTargetItem += (it, isOwn) =>
        {
            Debug.Log($"OrderItemSlot OnTargetItem: {it}, isOwn={isOwn}");
        };
    }

    private void SetRewardItemSlot(Item item)
    {
        var slot = Instantiate(rewardItemSlotPrefab, rewardItemList.transform);
        slot.SetRewardItem(item);
    }


    private void ClearTask()
    {
        characterImage.sprite = null;
        nameTitle.text = string.Empty;

        foreach (Transform child in deliveryItemList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in rewardItemList.transform)
        {
            Destroy(child.gameObject);
        }
    }
}