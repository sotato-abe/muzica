using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PropertyPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI diskText;
    [SerializeField] private TextMeshProUGUI keyText;

    public void SetMoney(int money)
    {
        moneyText.text = money.ToString();
    }
    public void SetDisk(int disk)
    {
        diskText.text = disk.ToString();
    }
    public void SetKey(int key)
    {
        keyText.text = key.ToString();
    }
}
