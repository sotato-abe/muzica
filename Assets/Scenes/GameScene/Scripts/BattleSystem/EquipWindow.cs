using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipWindow : MonoBehaviour
{
    [SerializeField] Image equipImage;
    [SerializeField] TextMeshProUGUI costLifeText;
    [SerializeField] TextMeshProUGUI costBatteryText;
    [SerializeField] TextMeshProUGUI costSoulText;
    [SerializeField] EquipmentInfo equipmentInfo;

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
        SetCosts(equipment.EquipmentBase.EnergyCostList);
    }

    private void SetCosts(List<EnergyCost> costs)
    {
        costLifeText.text = "0";
        costBatteryText.text = "0";
        costSoulText.text = "0";

        foreach (var cost in costs)
        {
            switch (cost.type)
            {
                case EnergyType.Life:
                    costLifeText.text = cost.val.ToString() ?? "0";
                    break;
                case EnergyType.Battery:
                    costBatteryText.text = cost.val.ToString() ?? "0";
                    break;
                case EnergyType.Soul:
                    costSoulText.text = cost.val.ToString() ?? "0";
                    break;
            }
        }
    }

    public void ResetSlot()
    {
        equipImage.gameObject.SetActive(false);
        costLifeText.text = "0";
        costBatteryText.text = "0";
        costSoulText.text = "0";
        equipmentInfo.gameObject.SetActive(false);
    }
}
