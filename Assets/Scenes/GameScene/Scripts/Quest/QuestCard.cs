using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestCard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questTitle;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] StoryQuestWindow storyQuestWindow;
    [SerializeField] SupplyQuestWindow supplyQuestWindow;
    [SerializeField] DeliveryQuestWindow deliveryQuestWindow;
    [SerializeField] ExterminationQuestWindow exterminationQuestWindow;
    [SerializeField] SpecialQuestWindow specialQuestWindow;

    private Quest currentQuest;

    public void SetQuest(Quest quest)
    {
        Debug.Log("Setting quest: " + (quest != null ? quest.Base.Name : "null"));
        currentQuest = quest;
        questTitle.text = quest.Base.Name;
        descriptionText.text = quest.Base.Description;

        SetTask();
    }

    private void SetTask()
    {
        ClearTask();
        switch (currentQuest.GetQuestType())
        {
            case QuestType.Story:
                var storyQuest = currentQuest as StoryQuest;
                storyQuestWindow.gameObject.SetActive(true);
                break;
            case QuestType.Supply:
                var supplyQuest = currentQuest as SupplyQuest;
                supplyQuestWindow.gameObject.SetActive(true);
                break;

            case QuestType.Delivery:
                var deliveryQuest = currentQuest as DeliveryQuest;
                deliveryQuestWindow.gameObject.SetActive(true);
                break;
            case QuestType.Extermination:
                var exterminationQuest = currentQuest as ExterminationQuest;
                exterminationQuestWindow.gameObject.SetActive(true);
                break;
            case QuestType.Special:
                var specialQuest = currentQuest as SpecialQuest;
                specialQuestWindow.gameObject.SetActive(true);
                break;

            default:
                Debug.LogWarning("Unknown quest type");
                break;
        }
    }

    private void ClearTask()
    {
        storyQuestWindow.gameObject.SetActive(false);
        supplyQuestWindow.gameObject.SetActive(false);
        deliveryQuestWindow.gameObject.SetActive(false);
        exterminationQuestWindow.gameObject.SetActive(false);
        specialQuestWindow.gameObject.SetActive(false);
    }
}
