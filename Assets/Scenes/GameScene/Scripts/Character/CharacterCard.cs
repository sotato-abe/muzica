using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class CharacterCard : MonoBehaviour
{
    Character character;
    [SerializeField] CardTitle cardTitle;
    [SerializeField] Image cardImage;
    [SerializeField] Image rarityFrame;
    [SerializeField] StatusLayer statusLayer;
    
    public void Setup(Character character)
    {
        this.character = character;
        cardTitle.SetCardTitle(character.Base.Rarity, character.Base.Name);
        cardImage.sprite = character.Base.Sprite;
        SetRarity(character.Base.Rarity);
        statusLayer.SetCharacterStatus(character);
    }

    private void SetRarity(RarityType rarity)
    {
        rarityFrame.color = rarity.GetRarityColor();
    }
}
