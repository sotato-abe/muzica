using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EnegyCostIcon : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] Image backImage;
    [SerializeField] TextMeshProUGUI valText;
    [SerializeField] Sprite lifeIcon;
    [SerializeField] Sprite batteryIcon;
    [SerializeField] Sprite soulIcon;

    Color32 lifeColor = new Color32(255, 101, 0, 200);
    Color32 batteryColor = new Color32(255, 164, 0, 255);
    Color32 soulColor = new Color32(154, 255, 0, 255);

    public void SetCost(EnegyCost enegyCost)
    {
        switch (enegyCost.type)
        {
            case EnegyType.Life:
                iconImage.sprite = lifeIcon;
                backImage.color = lifeColor;
                break;
            case EnegyType.Battery:
                iconImage.sprite = batteryIcon;
                backImage.color = batteryColor;
                break;
            case EnegyType.Soul:
                iconImage.sprite = soulIcon;
                backImage.color = soulColor;
                break;
            default:
                Debug.LogError("Unknown EnegyType");
                break;
        }

        valText.text = enegyCost.val.ToString();
    }
}