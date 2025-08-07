using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EnergyCountIcon : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] Image backImage;
    [SerializeField] TextMeshProUGUI valText;
    [SerializeField] Sprite lifeIcon;
    [SerializeField] Sprite batteryIcon;
    [SerializeField] Sprite soulIcon;
    [SerializeField] RectTransform backRectTransform;

    Color32 attackColor = new Color32(255, 101, 0, 255);
    Color32 recoveryColor = new Color32(0, 255, 219, 255);

    int widthPadding = 55;

    public void SetCount(EnergyCount energyCount)
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
            backImage.color = recoveryColor; // 回復ならオレンジ
        else
            backImage.color = attackColor; // 攻撃なら赤

        string val = "";
        if (energyCount.times > 1)
        {
            val += "x" + energyCount.times.ToString();
        }
        else
        {
            val = energyCount.val.ToString();
        }

        valText.text = val;
        ResizeIcon();
    }

    private void ResizeIcon()
    {
        // フォントのサイズを調整する処理
        float newWidth = valText.preferredWidth + widthPadding;
        float height = backRectTransform.sizeDelta.y;
        backRectTransform.sizeDelta = new Vector2(newWidth, height);
    }
}