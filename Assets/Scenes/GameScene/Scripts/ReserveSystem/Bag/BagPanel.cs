using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class BagPanel : Panel
{
    [SerializeField] InventoryWindow inventoryWindow;
    [SerializeField] TargetItemWindow targetItemWindow;
    [SerializeField] BagCategory bagCategory;
    [SerializeField] EquipmentWindow equipmentWindow;
    [SerializeField] PocketWindow pocketWindow;

    private void Start()
    {
        equipmentWindow.OnUpdateInventory += UpdateInvenotry;
        inventoryWindow.OnTargetItem += TargetItem;
        pocketWindow.OnTargetItem += TargetItem;
        bagCategory.OnChangeWindow += ChangeWindow;
        ChangeWindow(true);
    }

    public void UpdateInvenotry()
    {
        inventoryWindow.SetItems();
    }

    public void TargetItem(Item item)
    {
        targetItemWindow.TargetItem(item);
    }

    public void ChangeWindow(bool isEquipment)
    {
        equipmentWindow.gameObject.SetActive(isEquipment);
        pocketWindow.gameObject.SetActive(!isEquipment);
    }
}
