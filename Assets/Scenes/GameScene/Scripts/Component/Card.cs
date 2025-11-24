using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    [SerializeField] CardTitle cardTitle;
    [SerializeField] Image cardTypeIcon;
    [SerializeField] Image cardImage;

    public virtual void SetCard(Item item)
    {
        this.gameObject.SetActive(true);
        cardImage.sprite = item.Base.Sprite;
        cardTypeIcon.color = item.Base.Rarity.GetRarityColor();
        cardTitle.SetCardTitle(item.Base.Rarity, item.Base.Name);
    }

    public virtual void SetCommand(Command command)
    {
        this.gameObject.SetActive(true);
        cardImage.sprite = command.Base.Sprite;
        cardTypeIcon.color = command.Base.Rarity.GetRarityColor();
        cardTitle.SetCardTitle(command.Base.Rarity, command.Base.Name);
    }
}
