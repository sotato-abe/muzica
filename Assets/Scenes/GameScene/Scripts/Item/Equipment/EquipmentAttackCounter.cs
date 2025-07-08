using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EquipmentAttackCounter : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] TextMeshProUGUI valText;
    [SerializeField] TextMeshProUGUI timesText;
    [SerializeField] Sprite lifeIcon;
    [SerializeField] Sprite batteryIcon;
    [SerializeField] Sprite soulIcon;

    public void SetCounter(EnegyCount enegyCount)
    {
        valText.text = enegyCount.val.ToString();
        switch (enegyCount.type)
        {
            case EnegyType.Life:
                iconImage.sprite = lifeIcon;
                break;
            case EnegyType.Battery:
                iconImage.sprite = batteryIcon;
                break;
            case EnegyType.Soul:
                iconImage.sprite = soulIcon;
                break;
            default:
                Debug.LogError("Unknown EnegyType");
                break;
        }

        if (enegyCount.times > 1)
        {
            timesText.text = "* " + enegyCount.times.ToString();
        }
        else
        {
            timesText.text = "";
        }
    }
}