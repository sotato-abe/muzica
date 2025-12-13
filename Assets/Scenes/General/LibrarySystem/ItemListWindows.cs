using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class ItemListWindows : SelectWindow
{
    [SerializeField] private GameObject contentArea;
    [SerializeField] private SelectElement selectElementObject;
    [SerializeField] ConsumableCard consumableCard;
    [SerializeField] EquipmentCard equipmentCard;
    [SerializeField] TreasureCard treasureCard;

    private List<ConsumableBase> consumableBaseList;
    private List<EquipmentBase> equipmentBaseList;
    private List<TreasureBase> treasureBaseList;

    private ItemType currentItemType = ItemType.Consumable;

    private int consumabeleIndex = 0;
    private int equipmentIndex = 0;
    private int treasureIndex = 0;

    private void Start()
    {
        ChangeItemType(currentItemType);
        ChangeActiveWindow(false);
    }

    public void ChangeItemType(ItemType itemType)
    {
        switch (currentItemType)
        {
            case ItemType.Consumable:
                consumabeleIndex = GetCurrentIndex();
                break;
            case ItemType.Equipment:
                equipmentIndex = GetCurrentIndex();
                break;
            case ItemType.Treasure:
                treasureIndex = GetCurrentIndex();
                break;
        }
        currentItemType = itemType;
        SetListElement();
    }

    private void SetListElement()
    {
        foreach (Transform child in contentArea.transform)
        {
            Destroy(child.gameObject);
        }
        
        selectElements.Clear();

        int initialIndex = 0;
        switch (currentItemType)
        {
            case ItemType.Consumable:
                SetConsumableElements();
                initialIndex = consumabeleIndex;
                break;
            case ItemType.Equipment:
                SetEquipmentElements();
                initialIndex = equipmentIndex;
                break;
            case ItemType.Treasure:
                SetTreasureElements();
                initialIndex = treasureIndex;
                break;
        }
        
        if (selectElements.Count > 0)
        {
            initialIndex = Mathf.Clamp(initialIndex, 0, selectElements.Count - 1);
            selectElements[initialIndex].SetActiveCursol(true);
            TargetElement(initialIndex);
        }
    }

    private void SetConsumableElements()
    {
        // アイテムの状態（取得状態など）を表示するため、状態記録も取得する。
        consumableBaseList = ItemDatabase.Instance.GetAllConsumableBases();
        for (int i = 0; i < consumableBaseList.Count; i++)
        {
            ConsumableBase consumableBase = consumableBaseList[i];
            SelectElement newElement = Instantiate(selectElementObject, contentArea.transform);
            newElement.SetElementText(consumableBase.Name);
            newElement.SetIndex(i);
            newElement.SetActiveCursol(false);
            selectElements.Add(newElement);
        }
    }

    private void SetEquipmentElements()
    {
        equipmentBaseList = ItemDatabase.Instance.GetAllEquipmentBases();
        for (int i = 0; i < equipmentBaseList.Count; i++)
        {
            EquipmentBase equipmentBase = equipmentBaseList[i];
            SelectElement newElement = Instantiate(selectElementObject, contentArea.transform);
            newElement.SetElementText(equipmentBase.Name);
            newElement.SetIndex(i);
            newElement.SetActiveCursol(false);
            selectElements.Add(newElement);
        }
    }

    private void SetTreasureElements()
    {
        treasureBaseList = ItemDatabase.Instance.GetAllTreasureBases();
        for (int i = 0; i < treasureBaseList.Count; i++)
        {
            TreasureBase treasureBase = treasureBaseList[i];
            SelectElement newElement = Instantiate(selectElementObject, contentArea.transform);
            newElement.SetElementText(treasureBase.Name);
            newElement.SetIndex(i);
            newElement.SetActiveCursol(false);
            selectElements.Add(newElement);
        }
    }

    public override void TargetElement(int index)
    {
        TargetItemCard(index);
    }

    private void TargetItemCard(int index)
    {
        switch (currentItemType)
        {
            case ItemType.Consumable:
                treasureCard.gameObject.SetActive(false);
                equipmentCard.gameObject.SetActive(false);
                consumableCard.gameObject.SetActive(true);
                ConsumableBase consumableBase = consumableBaseList[index];
                Consumable consumable = new Consumable(consumableBase);
                consumableCard.SetCard(consumable);
                break;
            case ItemType.Equipment:
                consumableCard.gameObject.SetActive(false);
                treasureCard.gameObject.SetActive(false);
                equipmentCard.gameObject.SetActive(true);
                EquipmentBase equipmentBase = equipmentBaseList[index];
                Equipment equipment = new Equipment(equipmentBase);
                equipmentCard.SetCard(equipment);
                break;
            case ItemType.Treasure:
                consumableCard.gameObject.SetActive(false);
                equipmentCard.gameObject.SetActive(false);
                treasureCard.gameObject.SetActive(true);
                TreasureBase treasureBase = treasureBaseList[index];
                Treasure treasure = new Treasure(treasureBase);
                treasureCard.SetCard(treasure);
                break;
        }
    }
}
