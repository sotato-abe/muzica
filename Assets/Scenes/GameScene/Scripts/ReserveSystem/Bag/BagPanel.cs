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
    [SerializeField] EquipmentWindow equipmentWindow;

    private void Start()
    {
        equipmentWindow.OnUpdateInventory += UpdateInvenotry;
        inventoryWindow.OnTargetItem += TargetItem;
    }

    public void UpdateInvenotry()
    {
        inventoryWindow.SetItems();
    }

    public void TargetItem(Item item)
    {
        targetItemWindow.TargetItem(item);
    }
}
