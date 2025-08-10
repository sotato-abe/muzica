using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AttackCounter : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] TextMeshProUGUI valText;
    [SerializeField] TextMeshProUGUI timesText;
    [SerializeField] Sprite lifeIcon;
    [SerializeField] Sprite batteryIcon;
    [SerializeField] Sprite soulIcon;

    Color32 attackColor = new Color32(255, 101, 0, 255);
    Color32 recoveryColor = new Color32(0, 255, 219, 255);

    public void SetCounter(EnergyCount energyCount)
    {
        switch (energyCount.type)
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

        if (energyCount.isRecovery)
        {
            iconImage.color = recoveryColor;
            valText.color = recoveryColor;
            timesText.color = recoveryColor;
        }
        else
        {
            iconImage.color = attackColor;
            valText.color = attackColor;
            timesText.color = attackColor;
        }

        if (energyCount.times > 1)
        {
            timesText.text = "* " + energyCount.times.ToString();
        }
        else
        {
            timesText.text = "";
        }
        valText.text = energyCount.val.ToString();
    }
}