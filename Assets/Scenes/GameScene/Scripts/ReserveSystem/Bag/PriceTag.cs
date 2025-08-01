using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PriceTag : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI priceText;

    public void SetPrice(int? price, bool isOwn = true)
    {
        if (price == null || price <= 0)
        {
            gameObject.SetActive(false);
            priceText.text = "0";
            return;
        }

        gameObject.SetActive(true);
        int priceValue = isOwn ? price.Value / 2 : price.Value;
        priceText.text = priceValue.ToString();
    }
}
