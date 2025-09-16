using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MockItemBlock : MonoBehaviour
{
    [SerializeField] Image mockItemImage;
    [SerializeField] Image mockItemFrame;

    public void SetMockItem(Item item)
    {
        if (item == null) return;
        mockItemImage.sprite = item.Base.Sprite;
        Color frameColor = item.Base.itemType.GetItemTypeColor();
        mockItemFrame.color = frameColor;
    }

    private void ClearRewardItem()
    {
        mockItemImage.sprite = null;
        mockItemFrame.color = Color.clear;
    }
}