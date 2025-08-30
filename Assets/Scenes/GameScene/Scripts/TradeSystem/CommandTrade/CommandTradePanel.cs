using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class CommandTradePanel : Panel
{
    [SerializeField] TargetCommandWindow targetCommandWindow;
    [SerializeField] ShopCommandWindow shopCommandWindow;
    [SerializeField] BagCategory bagCategory;
    [SerializeField] StorageWindow storageWindow;
    [SerializeField] SlotSettingWindow slotSettingWindow;

    public delegate void OwnerMessageDelegate(TalkMessage message);
    public event OwnerMessageDelegate OnOwnerMessage;

    private void Start()
    {
        shopCommandWindow.OnTargetCommand += TargetCommand;
        shopCommandWindow.OnOwnerMessage += OwnerMessage;
        storageWindow.OnTargetCommand += TargetCommand;
        slotSettingWindow.OnTargetCommand += TargetCommand;
        bagCategory.OnChangeWindow += ChangeWindow;
        ChangeWindow(true);
    }

    public void TargetCommand(Command Command, bool isOwn = true)
    {
        targetCommandWindow.TargetCommand(Command, isOwn);
    }

    public void ChangeWindow(bool isBag)
    {
        storageWindow.gameObject.SetActive(isBag);
        slotSettingWindow.gameObject.SetActive(!isBag);
    }

    public void SetPoint(Point point)
    {
        shopCommandWindow.SetPoint(point);
    }

    public void OwnerMessage(TalkMessage message)
    {
        OnOwnerMessage?.Invoke(message);
    }
}
