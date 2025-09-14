using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class TalkPanel : Panel
{
    [SerializeField] QuestCard questCard;
    // [SerializeField] ShopQuestWindow shopQuestWindow;

    public void SetPoint(Point point)
    {
        Quest quest = point.GetActiveQuest();
        if (quest == null)
        {
            Debug.Log("No quest available at this point.");
            questCard.gameObject.SetActive(false);
            return;
        }
        questCard.gameObject.SetActive(true);
        questCard.SetQuest(quest);
    }
}
