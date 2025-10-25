using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipWindow : MonoBehaviour
{
    [SerializeField] Image equipImage;
    [SerializeField] Image equipStatusImage;

    [SerializeField] CostIconPrefab costIconPrefab;
    [SerializeField] GameObject costList;

    [SerializeField] EquipmentInfo equipmentInfo;
    [SerializeField] EquipStatusWindow equipStatusWindow;

    public void SetEquipment(Equipment equipment)
    {
        if (equipment == null || equipment.Base == null)
        {
            Debug.LogWarning("Equipment base is null, cannot set equipment.");
            ResetSlot();
            return;
        }
        equipImage.gameObject.SetActive(true);
        equipImage.sprite = equipment.Base.Sprite;
        equipmentInfo.SetInfo(equipment);
        SetCost(equipment.EquipmentBase.EnergyCostList);
    }

    private void SetCost(List<EnergyCost> energyCostList)
    {
        foreach (Transform child in costList.transform)
        {
            Destroy(child.gameObject);
        }

        // Costを表示する処理
        foreach (var cost in energyCostList)
        {
            CostIconPrefab newCost = Instantiate(costIconPrefab, costList.transform);
            newCost.SetCostIcon(cost);
        }
    }

    public void ResetSlot()
    {
        equipImage.gameObject.SetActive(false);
        equipStatusWindow.SetStatus(EquipStatusType.Empty);
        foreach (Transform child in costList.transform)
        {
            Destroy(child.gameObject);
        }
        equipmentInfo.ClearInfo();
    }

    public void SetStatusImage(bool canUse)
    {
        if (canUse)
        {
            equipStatusImage.gameObject.SetActive(false);
            equipStatusWindow.SetStatus(EquipStatusType.Active);
        }
        else
        {
            equipStatusImage.gameObject.SetActive(true);
            equipStatusWindow.SetStatus(EquipStatusType.EnergyOut);
        }
    }
}
