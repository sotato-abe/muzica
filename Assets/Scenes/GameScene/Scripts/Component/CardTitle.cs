using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardTitle : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI cardTitle;
    [SerializeField] RarityIcon cardRarity;
    [SerializeField] RectTransform backgroundRect;

    float padding = 30f;

    public void SetCardTitle(RarityType rarity, string title)
    {
        SetRarity(rarity);
        SetCardTitle(title);
    }

    private void SetRarity(RarityType type)
    {
        cardRarity.SetRarityIcon(type);
    }

    private void SetCardTitle(string title)
    {
        cardTitle.text = title;
        AdjustBackgroundSize();
    }

    private void AdjustBackgroundSize()
    {
        // テキストのサイズに基づいて背景のサイズを調整するロジックをここに追加できます
        backgroundRect.sizeDelta = new Vector2(cardTitle.preferredWidth + padding, backgroundRect.sizeDelta.y);
    }
}
