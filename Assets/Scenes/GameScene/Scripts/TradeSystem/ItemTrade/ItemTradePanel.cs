using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ItemTradePanel : TwoColumnPanel
{
    [SerializeField] CategorySwitch categorySwitch;
    [SerializeField] ItemBoxWindow itemBoxWindow;
    [SerializeField] EquipmentSimpleSlot rightHandSlot;
    [SerializeField] EquipmentSimpleSlot leftHandSlot;
    [SerializeField] PocketWindow pocketWindow;

    [SerializeField] ShopItemWindow shopItemWindow;

    public delegate void OwnerMessageDelegate(TalkMessage message);
    public event OwnerMessageDelegate OnOwnerMessage;

    public override void Start()
    {
        shopItemWindow.OnOwnerMessage += OwnerMessage;
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
        itemBoxWindow.gameObject.SetActive(isBag);
        pocketWindow.gameObject.SetActive(!isBag);
    }

    public void SetPoint(Point point)
    {
        shopItemWindow.SetPoint(point);
    }

    public void OwnerMessage(TalkMessage message)
    {
        OnOwnerMessage?.Invoke(message);
    }
}
