using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class BagPanel : SlidePanel
{
    [SerializeField] ItemBoxWindow itemBoxWindow;
    [SerializeField] TargetItemWindow targetItemWindow;
    [SerializeField] EquipmentWindow equipmentWindow;
    [SerializeField] PocketWindow pocketWindow;

    private void Start()
    {
        equipmentWindow.OnUpdateInventory += UpdateInvenotry;
        itemBoxWindow.OnTargetItem += TargetItem;
        pocketWindow.OnTargetItem += TargetItem;
    }

    public void UpdateInvenotry()
    {
        itemBoxWindow.SetItems();
    }

    public void TargetItem(Item item, bool isOwn = true)
    {
        targetItemWindow.TargetItem(item, isOwn);
    }
}
