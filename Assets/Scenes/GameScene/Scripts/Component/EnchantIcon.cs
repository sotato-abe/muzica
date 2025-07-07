using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EnchantIcon : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] TextMeshProUGUI valText;

    public void SetEnchant(Enchant enchant)
    {
        EnchantData data = EnchantDatabase.Instance?.GetData(enchant.Type);
        if (data == null)
        {
            Debug.LogWarning($"Enchant data not found for type: {enchant.Type}");
            return;
        }
        
        iconImage.sprite = data.icon;
        valText.text = enchant.Val.ToString();
    }
}