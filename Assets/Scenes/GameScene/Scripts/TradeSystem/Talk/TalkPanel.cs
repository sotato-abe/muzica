using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class TalkPanel : Panel
{
    [SerializeField] QuestCard questCard;
    [SerializeField] GameObject emptyAlert;
    [SerializeField] Button receiptButton;

    [SerializeField] TargetItemWindow targetItemWindow;

    public void SetPoint(Point point)
    {
        Quest quest = point.GetActiveQuest();
        if (quest == null)
        {
            Debug.Log("No quest available at this point.");
            emptyAlert.SetActive(true);
            questCard.gameObject.SetActive(false);
            receiptButton.interactable = false;
            return;
        }
        quest.Init();
        emptyAlert.SetActive(false);
        questCard.gameObject.SetActive(true);
        questCard.OnTargetItem += TargetItem;
        receiptButton.interactable = true;
        questCard.SetQuest(quest);
    }

    public void TargetItem(Item item, bool isOwn = true)
    {
        targetItemWindow.TargetItem(item, isOwn);
    }
}
