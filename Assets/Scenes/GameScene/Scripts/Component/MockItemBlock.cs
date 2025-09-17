using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MockItemBlock : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image mockItemImage;
    [SerializeField] Image mockItemFrame;

    private Item currentItem;

    public delegate void TargetItemDelegate(Item item);
    public event TargetItemDelegate OnTargetItem;

    public void SetMockItem(Item item)
    {
        if (item == null) return;
        currentItem = item;
        mockItemImage.sprite = item.Base.Sprite;
        Color frameColor = item.Base.itemType.GetItemTypeColor();
        mockItemFrame.color = frameColor;
    }

    private void ClearRewardItem()
    {
        mockItemImage.sprite = null;
        mockItemFrame.color = Color.clear;
    }

    public void TargetItem()
    {
        OnTargetItem?.Invoke(currentItem);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        OnTargetItem?.Invoke(currentItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnTargetItem?.Invoke(null);
    }
}