using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ShopQuestWindow : MonoBehaviour
{
    [SerializeField] QuestBlock questBlockPrefab;
    [SerializeField] GameObject questList;

    public void SetPoint(Point point)
    {
        ClearQuestList();
        if (point == null || point.Owner == null) return;

        List<Quest> quests = point.ShopQuests;
        Debug.Log("Quests Count: " + (quests != null ? quests.Count.ToString() : "null"));
        if (quests == null || quests.Count == 0) return;

        foreach (Quest quest in quests)
        {
            quest.isNew = false;
            CreateQuestBlock(quest);
        }
    }
    private void ClearQuestList()
    {
        foreach (Transform child in questList.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateQuestBlock(Quest quest)
    {
        QuestBlock questBlock = Instantiate(questBlockPrefab, questList.transform);
        questBlock.Setup(quest);
    }
}
