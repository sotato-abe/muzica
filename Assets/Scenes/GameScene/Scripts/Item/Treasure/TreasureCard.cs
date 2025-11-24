using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreasureCard : Card
{
    [SerializeField] TextMeshProUGUI description;

    public override void SetCard(Item item)
    {
        base.SetCard(item);
        Treasure treasure = item as Treasure;
        SetDescription(treasure.Base.Description);
    }

    public void SetDescription(string descriptionText)
    {
        description.text = descriptionText;
    }
}
