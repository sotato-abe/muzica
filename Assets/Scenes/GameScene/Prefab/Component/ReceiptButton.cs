using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReceiptButton : Button
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private TextMeshProUGUI buttonText;

    private void OnEnable()
    {
        this.gameObject.transform.localScale = Vector3.one;
    }

    public void SetButton(string text)
    {
        buttonText.text = text;
    }
}