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
    [SerializeField] EquipmentSimpleWindow equipmentWindow;
    [SerializeField] PocketWindow pocketWindow;

    private void Start()
    {
        shopItemWindow.OnTargetItem += TargetItem;
        inventoryWindow.OnTargetItem += TargetItem;
        equipmentWindow.OnTargetItem += TargetItem;
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
        equipmentWindow.gameObject.SetActive(!isBag);
        pocketWindow.gameObject.SetActive(!isBag);
    }

    public void SetShopItems(List<Item> items)
    {
        shopItemWindow.SetItems(items);
    }
}
