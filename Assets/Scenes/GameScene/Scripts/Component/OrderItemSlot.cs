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

    public void SetOrderItem(ItemBase item)
    {
        if (item == null) return;
        orderitemImage.sprite = item.Sprite;
        // UIの更新などの処理をここに追加
    }

    public void OnDrop(PointerEventData eventData)
    {

    }

    private void ClearOrderItem()
    {
        orderitemImage.sprite = null;
        isSet = false;
    }
}
