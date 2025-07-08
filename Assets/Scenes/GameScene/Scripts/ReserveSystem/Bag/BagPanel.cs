using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class BagPanel : Panel
{
    [SerializeField] InventoryWindow inventoryWindow;
    [SerializeField] EquipmentWindow equipmentWindow;

    private void Start()
    {
        equipmentWindow.OnUpdateInventory += UpdateInvenotry;
    }

    public void UpdateInvenotry()
    {
        inventoryWindow.SetItems();
    }
}
