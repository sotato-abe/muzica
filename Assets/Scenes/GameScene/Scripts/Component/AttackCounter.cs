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

    Color32 damageColor = new Color32(255, 101, 0, 255);
    Color32 recoveryColor = new Color32(0, 255, 219, 255);

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
        if (enegyCount.isRecovery)
        {
            iconImage.color = recoveryColor;
            valText.color = recoveryColor;
            timesText.color = recoveryColor;
        }
        else
        {
            iconImage.color = damageColor;
            valText.color = damageColor;
            timesText.color = damageColor;
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