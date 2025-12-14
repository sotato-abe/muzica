using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReceiptButton : Button
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private TextMeshProUGUI buttonText;

    protected override void OnEnable()
    {
        base.OnEnable(); // 基底クラスのOnEnable()を呼び出し
        this.gameObject.transform.localScale = Vector3.one;
    }

    public void SetButton(string text)
    {
        buttonText.text = text;
    }
}