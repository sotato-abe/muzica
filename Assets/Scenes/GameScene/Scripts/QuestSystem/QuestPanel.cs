using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class QuestPanel : TwoColumnPanel
{
    [SerializeField] TargetItemWindow targetItemWindow;
    [SerializeField] TargetCommandWindow targetCommandWindow;

    [SerializeField] BagCategory bagCategory;
    [SerializeField] ItemBoxWindow itemBoxWindow;
    [SerializeField] StorageWindow storageWindow;

    [SerializeField] QuestCard questCard;
    [SerializeField] GameObject emptyAlert;

    private Point currentPoint;
    private Quest currentQuest;

    public delegate void OwnerMessageDelegate(TalkMessage message);
    public event OwnerMessageDelegate OnOwnerMessage;

    public override void Start()
    {
        itemBoxWindow.OnTargetItem += TargetItem;
        storageWindow.OnTargetCommand += TargetCommand;
        bagCategory.OnChangeWindow += ChangeWindow;
        questCard.OnTargetItem += TargetItem;
        questCard.OnOwnerMessage += OwnerMessage;
        questCard.OnReceiptQuest += ReceiptQuest;
        ChangeWindow(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            bagCategory.SwitchActiveButton();
        }
    }

    public void TargetItem(Item item, bool isOwn = true)
    {
        targetItemWindow.TargetItem(item, isOwn);
    }

    public void TargetCommand(Command Command, bool isOwn = true)
    {
        targetCommandWindow.TargetCommand(Command, isOwn);
    }

    public void ChangeWindow(bool isBag)
    {
        itemBoxWindow.gameObject.SetActive(isBag);
        storageWindow.gameObject.SetActive(!isBag);
    }

    public void SetPoint(Point point)
    {
        currentPoint = point;
    }

    public void SetQuest(Quest quest)
    {
        currentQuest = quest;
        if (quest == null)
        {
            emptyAlert.SetActive(true);
            questCard.gameObject.SetActive(false);
            return;
        }
        quest.Init();
        emptyAlert.SetActive(false);
        questCard.gameObject.SetActive(true);
        questCard.OnTargetItem += TargetItem;
        questCard.SetQuest(quest);
    }

    public void OwnerMessage(TalkMessage message)
    {
        OnOwnerMessage?.Invoke(message);
    }

    public void ReceiptQuest(Quest quest)
    {
        if (currentPoint != null)
        {
            currentPoint.ShopQuests.Remove(quest); // ポイントのアイテムリストから削除
        }
        questCard.gameObject.SetActive(false);
        emptyAlert.SetActive(true);
        UpdateUI();
    }

    public bool HasQuest()
    {
        return currentQuest != null;
    }

    public void UpdateUI()
    {
        itemBoxWindow.SetItems();
        storageWindow.SetCommands();
    }
}
