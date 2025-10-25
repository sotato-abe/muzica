using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ItemTradePanel : TwoColumnPanel
{
    [SerializeField] BagCategory bagCategory;
    [SerializeField] InventoryWindow inventoryWindow;
    [SerializeField] EquipmentSimpleSlot rightHandSlot;
    [SerializeField] EquipmentSimpleSlot leftHandSlot;
    [SerializeField] PocketWindow pocketWindow;
    [SerializeField] TargetItemWindow targetItemWindow;

    [SerializeField] ShopItemWindow shopItemWindow;

    public delegate void OwnerMessageDelegate(TalkMessage message);
    public event OwnerMessageDelegate OnOwnerMessage;

    private void Start()
    {
        shopItemWindow.OnTargetItem += TargetItem;
        shopItemWindow.OnOwnerMessage += OwnerMessage;
        inventoryWindow.OnTargetItem += TargetItem;
        rightHandSlot.OnTargetItem += TargetItem;
        leftHandSlot.OnTargetItem += TargetItem;
        pocketWindow.OnTargetItem += TargetItem;
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

    public void TargetItem(Item item, bool isOwn = true)
    {
        targetItemWindow.TargetItem(item, isOwn);
    }

    public void ChangeWindow(bool isBag)
    {
        SoundSystem.Instance.PlaySE(SeType.Select);
        inventoryWindow.gameObject.SetActive(isBag);
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
