using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ItemTradePanel : Panel
{
    [SerializeField] ShopItemWindow shopItemWindow;
    [SerializeField] TargetItemWindow targetItemWindow;
    [SerializeField] BagCategory bagCategory;
    [SerializeField] InventoryWindow inventoryWindow;
    [SerializeField] EquipmentSimpleSlot rightHandSlot;
    [SerializeField] EquipmentSimpleSlot leftHandSlot;
    [SerializeField] PocketWindow pocketWindow;

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

    public void TargetItem(Item item, bool isOwn = true)
    {
        targetItemWindow.TargetItem(item, isOwn);
    }

    public void ChangeWindow(bool isBag)
    {
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
