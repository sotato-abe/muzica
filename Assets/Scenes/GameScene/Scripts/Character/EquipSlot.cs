using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class EquipSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image equipBlock;
    [SerializeField] Image equipImage;
    [SerializeField] Image rarityFrameImage;
    public Item Item { get; set; }

    public void SetEquip(Equipment equipment)
    {
        if (equipment == null)
        {
            ClearEquip();
            return;
        }
        Item = equipment;
        equipImage.sprite = equipment.Base.Sprite;
        equipBlock.gameObject.SetActive(true);
        rarityFrameImage.color = equipment.Base.Rarity.GetRarityColor();
    }

    public void ClearEquip()
    {
        Item = null;
        equipBlock.gameObject.SetActive(false);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Item != null)
        TooltipUI.Instance.TargetItem(Item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // if (Item != null)
        TooltipUI.Instance.ClearTargetItem();
    }
}
