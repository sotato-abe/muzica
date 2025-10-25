using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class BagPanel : SlidePanel
{
    [SerializeField] InventoryWindow inventoryWindow;
    [SerializeField] TargetItemWindow targetItemWindow;
    [SerializeField] EquipmentWindow equipmentWindow;
    [SerializeField] PocketWindow pocketWindow;

    private void Start()
    {
        equipmentWindow.OnUpdateInventory += UpdateInvenotry;
        equipmentWindow.OnTargetItem += TargetItem;
        inventoryWindow.OnTargetItem += TargetItem;
        pocketWindow.OnTargetItem += TargetItem;
    }

    public void UpdateInvenotry()
    {
        inventoryWindow.SetItems();
    }

    public void TargetItem(Item item, bool isOwn = true)
    {
        targetItemWindow.TargetItem(item, isOwn);
    }
}
