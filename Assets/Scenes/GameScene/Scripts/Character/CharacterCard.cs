using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

// バックで使用するアイテムのクラス
// 装備、消耗品、トレジャーをすべて受け入れてバックに表示するためのクラス
public class CharacterCard : MonoBehaviour
{
    Character character;
    [SerializeField] RarityIcon cardRarity;
    [SerializeField] TextMeshProUGUI characterName;
    [SerializeField] Image characterImage;
    [SerializeField] GameObject rarityStarSprite;
    [SerializeField] GameObject rarityList;
    
    public void Setup(Character character)
    {
        this.character = character;
        characterName.text = character.Base.Name;
        characterImage.sprite = character.Base.Sprite;
        SetRarity((int)character.Base.Rarity + 1);
    }

    private void SetRarity(int level)
    {
        cardRarity.SetRarityIcon(character.Base.Rarity);
        // レベルに応じて星を表示
        foreach (Transform child in rarityList.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < level; i++)
        {
            GameObject star = Instantiate(rarityStarSprite, rarityList.transform);
            star.SetActive(true);
            star.transform.localScale = Vector3.one; // スケールをリセット
        }
    }
}
