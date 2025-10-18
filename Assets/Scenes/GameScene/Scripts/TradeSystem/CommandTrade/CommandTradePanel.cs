using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class CommandTradePanel : TwoColumnPanel
{
    [SerializeField] TargetCommandWindow targetCommandWindow;

    [SerializeField] BagCategory bagCategory;
    [SerializeField] StorageWindow storageWindow;
    [SerializeField] SlotSettingWindow slotSettingWindow;

    [SerializeField] ShopCommandWindow shopCommandWindow;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            bagCategory.SwitchActiveButton();
        }
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
