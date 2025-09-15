using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestCard : MonoBehaviour
{
    [SerializeField] Image clientImage;
    [SerializeField] Image clientFrame;
    [SerializeField] TextMeshProUGUI clientNameTitle;
    [SerializeField] TextMeshProUGUI clientAddressTitle;

    [SerializeField] RarityIcon questRarity;
    [SerializeField] TextMeshProUGUI questTitle;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] StoryQuestTask storyQuestTask;
    [SerializeField] SupplyQuestTask supplyQuestTask;
    [SerializeField] DeliveryQuestTask deliveryQuestTask;
    [SerializeField] ExterminationQuestTask exterminationQuestTask;
    [SerializeField] SpecialQuestTask specialQuestTask;

    private Quest currentQuest;

    public void SetQuest(Quest quest)
    {
        if (quest == null) return;

        currentQuest = quest;
        questTitle.text = quest.Base.Title;
        descriptionText.text = quest.Base.Description;
        questRarity.SetRarityIcon(quest.Base.Rarity);

        SetTask();
    }

    private void SetTask()
    {
        ClearTask();
        switch (currentQuest.GetQuestType())
        {
            case QuestType.Story:
                var storyQuest = currentQuest as StoryQuest;
                storyQuestTask.gameObject.SetActive(true);
                break;
            case QuestType.Supply:
                var supplyQuest = currentQuest as SupplyQuest;
                supplyQuestTask.gameObject.SetActive(true);
                break;

            case QuestType.Delivery:
                var deliveryQuest = currentQuest as DeliveryQuest;
                deliveryQuestTask.gameObject.SetActive(true);
                deliveryQuestTask.SetDeliveryTask(deliveryQuest);
                break;
            case QuestType.Extermination:
                var exterminationQuest = currentQuest as ExterminationQuest;
                exterminationQuestTask.gameObject.SetActive(true);
                break;
            case QuestType.Special:
                var specialQuest = currentQuest as SpecialQuest;
                specialQuestTask.gameObject.SetActive(true);
                break;

            default:
                Debug.LogWarning("Unknown quest type");
                break;
        }
    }

    private void ClearTask()
    {
        storyQuestTask.gameObject.SetActive(false);
        supplyQuestTask.gameObject.SetActive(false);
        deliveryQuestTask.gameObject.SetActive(false);
        exterminationQuestTask.gameObject.SetActive(false);
        specialQuestTask.gameObject.SetActive(false);
    }
}
