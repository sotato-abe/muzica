using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CurrencyPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI discText;
    [SerializeField] private TextMeshProUGUI keyText;

    public void SetCoin(int coin)
    {
        coinText.text = coin.ToString();
    }
    public void SetDisc(int disc)
    {
        discText.text = disc.ToString();
    }
    public void SetKey(int key)
    {
        keyText.text = key.ToString();
    }
}
