using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestCard : MonoBehaviour
{
    [SerializeField] RarityIcon questRarity;
    [SerializeField] TextMeshProUGUI questTitle;
    [SerializeField] TextMeshProUGUI descriptionText;

    [SerializeField] StoryQuestTask storyQuestTask;
    [SerializeField] ShippingQuestTask shippingQuestTask;
    [SerializeField] DeliveryQuestTask deliveryQuestTask;
    [SerializeField] ExterminationQuestTask exterminationQuestTask;
    [SerializeField] WorkQuestTask workQuestTask;
    [SerializeField] SpecialQuestTask specialQuestTask;

    [SerializeField] ReceiptButton receiptButton;

    public delegate void OwnerMessageDelegate(TalkMessage message);
    public event OwnerMessageDelegate OnOwnerMessage;

    public delegate void ReceiptQuest(Quest quest);
    public event ReceiptQuest OnReceiptQuest;

    private Quest currentQuest;

    public void SetQuest(Quest quest)
    {
        ClearTask();
        receiptButton.gameObject.SetActive(false);
        if (quest == null) return;

        currentQuest = quest;
        questTitle.text = quest.Base.Title;
        descriptionText.text = quest.Base.Description;
        questRarity.SetRarityIcon(quest.Base.Rarity);

        SetTask();
    }

    private void SetTask()
    {
        switch (currentQuest.GetQuestType())
        {
            case QuestType.Story:
                var storyQuest = currentQuest as StoryQuest;
                storyQuestTask.gameObject.SetActive(true);
                break;
            case QuestType.Delivery:
                var deliveryQuest = currentQuest as DeliveryQuest;
                if (deliveryQuest.isShipping)
                {
                    shippingQuestTask.gameObject.SetActive(true);
                    shippingQuestTask.SetShippingTask(deliveryQuest);
                    break;
                }
                else
                {
                    deliveryQuestTask.gameObject.SetActive(true);
                    deliveryQuestTask.SetDeliveryTask(deliveryQuest);
                    receiptButton.gameObject.SetActive(true);
                    break;
                }
            case QuestType.Extermination:
                var exterminationQuest = currentQuest as ExterminationQuest;
                exterminationQuestTask.gameObject.SetActive(true);
                break;
            case QuestType.Work:
                var workQuest = currentQuest as WorkQuest;
                workQuestTask.SetWorkTask(workQuest);
                workQuestTask.gameObject.SetActive(true);
                receiptButton.gameObject.SetActive(true);
                break;
            case QuestType.Special:
                var specialQuest = currentQuest as SpecialQuest;
                specialQuestTask.gameObject.SetActive(true);
                break;

            default:
                Debug.LogWarning("Unknown quest type");
                break;
        }
        SetReceiptTypeButton();
    }

    private void SetReceiptTypeButton()
    {
        if (currentQuest == null || currentQuest.isCompleted)
        {
            receiptButton.gameObject.SetActive(false);
            return;
        }
        QuestType type = currentQuest.GetQuestType();
        if (type == QuestType.Story || type == QuestType.Special)
        {
            return;
        }
        string buttonText = type.GetQuestTypeReceiptString();
        receiptButton.SetButton(buttonText);
    }

    public void ActiveButtonStatus()
    {
        receiptButton.gameObject.SetActive(true);
    }

    public void ReceiptTask()
    {
        // クエスト受領の処理をここに実装
        switch (currentQuest.GetQuestType())
        {
            case QuestType.Delivery:
                ReceiptDeliveryTask();
                break;
            case QuestType.Extermination:
                break;
            case QuestType.Work:
                ReceiptWorkTask();
                break;
            case QuestType.Special:
                break;
            default:
                Debug.LogWarning("Unknown quest type");
                break;
        }
        QuestDatabase.Instance.MarkQuestAsFinished(currentQuest.Base);
        receiptButton.gameObject.SetActive(false);
    }

    private void ReceiptDeliveryTask()
    {
        DeliveryQuest deliveryQuest = currentQuest as DeliveryQuest;
        if (deliveryQuest.isShipping)
        {
            List<Item> deliveryItems = deliveryQuest.DeliveryItemList;
            bool hasSpace = PlayerController.Instance.HasBagSpace(deliveryItems.Count);
            if (!hasSpace)
            {
                OwnerMessage(new TalkMessage(MessageType.Other, MessagePanelType.Default, "バッグに空きが無いよ"));
                return;
            }
            foreach (Item item in deliveryItems)
            {
                PlayerController.Instance.AddItemToBag(item);
            }
            OwnerMessage(new TalkMessage(MessageType.Other, MessagePanelType.Default, "よろしく頼むよ"));
            OnReceiptQuest?.Invoke(currentQuest);
            // クエスト実行中のステータスをつける。
            currentQuest.Status = QuestStatus.InProgress;
        }
        else
        {
            // 配達するアイテムをバッグから削除（コントローラー側で一気に削除トライ（bool）ができるようにする）
            List<Item> deliveryItems = deliveryQuest.DeliveryItemList;
            foreach (Item item in deliveryItems)
            {
                PlayerController.Instance.RemoveItemFromBag(item);
            }

            List<Item> rewardItems = deliveryQuest.RewardItems;
            foreach (Item item in rewardItems)
            {
                PlayerController.Instance.AddItemToBag(item);
            }
            PlayerController.Instance.AddCoin(deliveryQuest.DeliveryQuestBase.CoinPrice);
            PlayerController.Instance.AddDisc(deliveryQuest.DeliveryQuestBase.DiscPrice);

            OwnerMessage(new TalkMessage(MessageType.Other, MessagePanelType.Default, "ありがとうよ"));
            OnReceiptQuest?.Invoke(currentQuest);
            currentQuest.isCompleted = true;
            currentQuest.Status = QuestStatus.Completed;
        }
    }

    private void ReceiptWorkTask()
    {
        WorkQuest workQuest = currentQuest as WorkQuest;
        List<Item> rewardItems = workQuest.RewardItems;
        // 作業クエストの受領処理をここに実装
        foreach (Item item in rewardItems)
        {
            PlayerController.Instance.AddItemToBag(item);
        }
        if (workQuest.WorkQuestBase.CoinPrice > 0)
        {
            PlayerController.Instance.AddCoin(workQuest.WorkQuestBase.CoinPrice);
        }
        if (workQuest.WorkQuestBase.DiscPrice > 0)
        {
            PlayerController.Instance.AddDisc(workQuest.WorkQuestBase.DiscPrice);
        }
        AgeTimePanel.Instance.PassageOfMonth(workQuest.WorkQuestBase.Month);
        OnReceiptQuest?.Invoke(currentQuest);
        currentQuest.isCompleted = true;
    }

    private void ClearTask()
    {
        storyQuestTask.gameObject.SetActive(false);
        shippingQuestTask.gameObject.SetActive(false);
        deliveryQuestTask.gameObject.SetActive(false);
        exterminationQuestTask.gameObject.SetActive(false);
        workQuestTask.gameObject.SetActive(false);
        specialQuestTask.gameObject.SetActive(false);
    }
    
    public void OwnerMessage(TalkMessage message)
    {
        OnOwnerMessage?.Invoke(message);
    }
}
