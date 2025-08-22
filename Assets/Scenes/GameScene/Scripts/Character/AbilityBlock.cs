using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

// バックで使用するアイテムのクラス
// 装備、消耗品、トレジャーをすべて受け入れてバックに表示するためのクラス
public class AbilityBlock : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Ability Ability { get; set; }
    [SerializeField] Image image;
    [SerializeField] Image frame;
    [SerializeField] TextMeshProUGUI title;

    public void Setup(Ability ability)
    {
        Ability = ability;
        image.sprite = Ability.Base.Sprite;
        title.SetText(Ability.Base.Name);
        SetRarity(Ability.Base.Rarity);
    }

    public void SetRarity(RarityType type)
    {
        frame.color = type.GetRarityColor();
        // title.color = type.GetRarityColor();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Handle pointer enter
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Handle pointer exit
    }
}
