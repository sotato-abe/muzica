using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI cardName;
    [SerializeField] RarityIcon questRarity;
    [SerializeField] public Image cardIcon;
    [SerializeField] public Image cardTypeColor;
    [SerializeField] public Image cardImage;

    public void SetRarity(RarityType type)
    {
        questRarity.SetRarityIcon(type);
    }

    public void SetCardType(ItemType type)
    {
        cardTypeColor.color = type.GetItemTypeColor();
    }
}
