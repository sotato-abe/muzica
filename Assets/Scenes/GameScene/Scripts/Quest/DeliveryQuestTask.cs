using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeliveryQuestTask : MonoBehaviour
{
    [Header("Task")] // タスク
    [SerializeField] Image characterImage;
    [SerializeField] Image characterFrame;
    [SerializeField] TextMeshProUGUI nameTitle;
    [SerializeField] TextMeshProUGUI addressTitle;

    // ここにデリバリークエストタスクのロジックを実装
    public void SetDeliveryTask(DeliveryQuest quest)
    {
        if (quest == null) return;
        ClearTask();
        // サプライクエストのタスク設定ロジックをここに実装
        SetDeliveryCharacter(quest);
        foreach (var item in quest.DeliveryQuestBase.DeliveryItemList)
        {
            SetDeliveryItemSlot(item);
        }
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

    private void SetDeliveryItemSlot(Treasure item)
    {
        var slot = Instantiate(rewardItemPrefab, deliveryItemList.transform);
        slot.SetMockItem(item);
        // slot.OnTargetItem += (it, isOwn) =>
        // {
        //     Debug.Log($"OrderItemSlot OnTargetItem: {it}, isOwn={isOwn}");
        // };
    }


    private void ClearTask()
    {
        characterImage.sprite = null;
        nameTitle.text = string.Empty;

        foreach (Transform child in deliveryItemList.transform)
        {
            Destroy(child.gameObject);
        }
    }
}