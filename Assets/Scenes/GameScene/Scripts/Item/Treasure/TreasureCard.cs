using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreasureCard : Card
{
    [SerializeField] TextMeshProUGUI description;

    public void SetTreasureDetail(Treasure treasure)
    {
        this.gameObject.SetActive(true);
        SetRarity(treasure.Base.Rarity);
        description.text = treasure.Base.Description;
        cardName.text = treasure.Base.Name;
        cardImage.sprite = treasure.Base.Sprite;
        cardImage.color = new Color(1, 1, 1, 1);
    }
}
