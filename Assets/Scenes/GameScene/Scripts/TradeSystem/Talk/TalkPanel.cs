using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class TalkPanel : Panel
{
    [SerializeField] TargetItemWindow targetItemWindow;
    [SerializeField] TargetCommandWindow targetCommandWindow;

    [SerializeField] BagCategory bagCategory;
    [SerializeField] InventoryWindow inventoryWindow;
    [SerializeField] StorageWindow storageWindow;

    [SerializeField] QuestCard questCard;
    [SerializeField] GameObject emptyAlert;

    private Point currentPoint;

    public delegate void OwnerMessageDelegate(TalkMessage message);
    public event OwnerMessageDelegate OnOwnerMessage;

    private void Start()
    {
        inventoryWindow.OnTargetItem += TargetItem;
        storageWindow.OnTargetCommand += TargetCommand;
        bagCategory.OnChangeWindow += ChangeWindow;
        questCard.OnTargetItem += TargetItem;
        questCard.OnOwnerMessage += OwnerMessage;
        questCard.OnReceiptQuest += ReceiptQuest;
        ChangeWindow(true);
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
        inventoryWindow.gameObject.SetActive(isBag);
        storageWindow.gameObject.SetActive(!isBag);
    }

    public void SetPoint(Point point)
    {
        currentPoint = point;
        Quest quest = point.GetActiveQuest();
        if (quest == null)
        {
            OwnerMessage(new TalkMessage(MessageType.Other, MessagePanelType.Default, "今は頼みごとはないかな"));
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
        currentPoint.ShopQuests.Remove(quest); // ポイントのアイテムリストから削除
        questCard.gameObject.SetActive(false);
        emptyAlert.SetActive(true);
        UpdateUI();
    }

    public void UpdateUI()
    {
        inventoryWindow.SetItems();
        storageWindow.SetCommands();
    }
}
