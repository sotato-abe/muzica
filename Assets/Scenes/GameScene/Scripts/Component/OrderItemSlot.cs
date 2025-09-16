using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OrderItemSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] Image orderitemImage;

    private bool isSet = false;

    public delegate void TargetItemDelegate(Item item, bool isOwn = true);
    public event TargetItemDelegate OnTargetItem;

    private Color activeTransparency = new Color(1f, 1f, 1f, 1f);
    private Color inactiveTransparency = new Color(1f, 1f, 1f, 0.3f);

    public void SetOrderItem(ItemBase item)
    {
        if (item == null) return;
        orderitemImage.sprite = item.Sprite;
        orderitemImage.color = activeTransparency;
        // UIの更新などの処理をここに追加
    }

    public void OnDrop(PointerEventData eventData)
    {
        
    }

    private void ClearOrderItem()
    {
        orderitemImage.sprite = null;
    }
}
