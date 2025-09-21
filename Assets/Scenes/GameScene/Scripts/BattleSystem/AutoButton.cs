using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AutoButton : MonoBehaviour
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private TextMeshProUGUI buttonText;

    bool isAuto = false;
    public bool IsAuto { get { return isAuto; } }

    Color activeColor = new Color(0 / 255f, 211f / 255f, 205f / 255f, 255f / 255f);
    Color stopColor = new Color(95f / 255f, 0f / 255f, 255f / 255f, 255f / 255f);

    private void OnEnable()
    {
        this.gameObject.transform.localScale = Vector3.one;
        ChangeButtonType(isAuto);
    }

    public void SwitchButtonType()
    {
        isAuto = !isAuto;
        ChangeButtonType(isAuto);
    }

    public void ChangeButtonType(bool isAuto)
    {
        this.isAuto = isAuto;
        if (isAuto)
        {
            buttonText.text = "Auto";
            buttonImage.color = activeColor;
        }
        else
        {
            buttonText.text = "Manual";
            buttonImage.color = stopColor;
        }
    }
}