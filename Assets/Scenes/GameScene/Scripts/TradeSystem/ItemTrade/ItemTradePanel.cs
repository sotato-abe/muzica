using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ItemTradePanel : Panel
{
    public UnityAction OnInsufficientCoin; // お金が足りない時に呼び出される。
    [SerializeField] ShopItemWindow shopItemWindow;
    [SerializeField] TargetItemWindow targetItemWindow;
    [SerializeField] BagCategory bagCategory;
    [SerializeField] InventoryWindow inventoryWindow;
    [SerializeField] EquipmentSimpleSlot rightHandSlot;
    [SerializeField] EquipmentSimpleSlot leftHandSlot;
    [SerializeField] PocketWindow pocketWindow;

    private void Start()
    {
        shopItemWindow.OnTargetItem += TargetItem;
        inventoryWindow.OnTargetItem += TargetItem;
        rightHandSlot.OnTargetItem += TargetItem;
        leftHandSlot.OnTargetItem += TargetItem;
        pocketWindow.OnTargetItem += TargetItem;
        bagCategory.OnChangeWindow += ChangeWindow;
        shopItemWindow.OnInsufficientCoin += InsufficientCoin;
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

    private void InsufficientCoin()
    {
        OnInsufficientCoin?.Invoke();
    }
}
