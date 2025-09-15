using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencyVal : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI priceText;

    public void SetCurrencyVal(int val)
    {
        if (priceText == null)
        {
            this.gameObject.SetActive(false);
            return;
        }
        this.gameObject.SetActive(true);
        priceText.text = val.ToString("N0");
    }
}
