using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class EquipSlot : MonoBehaviour
{
    [SerializeField] Image equipBlock;
    [SerializeField] Image equipImage;
    [SerializeField] Image rarityFrameImage;

    public void SetEquip(Equipment equipment)
    {
        if(equipment == null)
        {
            ClearEquip();
            return;
        }
        equipImage.sprite = equipment.Base.Sprite;
        rarityFrameImage.color = equipment.Base.Rarity.GetRarityColor();
        equipBlock.gameObject.SetActive(true);
    }

    public void ClearEquip()
    {
        equipBlock.gameObject.SetActive(false);
    }
}
