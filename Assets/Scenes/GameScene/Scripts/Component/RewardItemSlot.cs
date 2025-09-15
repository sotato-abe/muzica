using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RewardItemSlot : MonoBehaviour
{
    [SerializeField] Image orderitemImage;
    [SerializeField] Image rewarditemFrame;

    public void SetRewardItem(Item item)
    {
        if (item == null) return;
        orderitemImage.sprite = item.Base.Sprite;
        Color frameColor = item.Base.itemType.GetItemTypeColor();
        rewarditemFrame.color = frameColor;
    }

    private void ClearRewardItem()
    {
        orderitemImage.sprite = null;
        rewarditemFrame.color = Color.clear;
    }
}