using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class CommandTradePanel : TwoColumnPanel
{
    [SerializeField] TargetCommandWindow targetCommandWindow;

    [SerializeField] CategorySwitch categorySwitch;
    [SerializeField] CommandBoxWindow commandBoxWindow;
    [SerializeField] SlotSettingWindow slotSettingWindow;

    [SerializeField] ShopCommandWindow shopCommandWindow;

    public delegate void OwnerMessageDelegate(TalkMessage message);
    public event OwnerMessageDelegate OnOwnerMessage;

    public override void Start()
    {
        shopCommandWindow.OnOwnerMessage += OwnerMessage;
        categorySwitch.OnChangeWindow += ChangeWindow;
        ChangeWindow(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            categorySwitch.SwitchActiveButton();
        }
    }

    public void ChangeWindow(bool isBag)
    {
        SoundSystem.Instance.PlaySE(SeType.Select);
        commandBoxWindow.gameObject.SetActive(isBag);
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
