using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CostIconPrefab : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI valText;
    [SerializeField] Image iconImage;
    [SerializeField] Image backImage;
    [SerializeField] Sprite lifeIcon;
    [SerializeField] Sprite batteryIcon;
    [SerializeField] Sprite soulIcon;

    public void SetCostIcon(EnergyCost cost)
    {
        if (cost == null)
            return;

        valText.text = cost.val.ToString();
        SetType(cost.type);
    }
    
    private void SetType(EnergyType type)
    {
        Color32 energyColor = type.GetEnergyColor();
        backImage.color = energyColor;

        switch (type)
        {
            case EnergyType.Life:
                iconImage.sprite = lifeIcon;
                break;
            case EnergyType.Battery:
                iconImage.sprite = batteryIcon;
                break;
            case EnergyType.Soul:
                iconImage.sprite = soulIcon;
                break;
            default:
                Debug.LogError("Unknown EnergyType");
                break;
        }
    }
}
