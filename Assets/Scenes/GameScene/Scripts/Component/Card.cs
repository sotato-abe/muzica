using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI cardName;
    [SerializeField] public Image cardIcon;
    [SerializeField] public Image cardFrame;
    [SerializeField] public Image cardImage;

    public void SetRarity(RarityType type)
    {
        cardFrame.color = type.GetRarityColor();
    }
}
