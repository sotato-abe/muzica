using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OrderItemSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image orderitemImage;

    private Item currentItem;
    private bool isSet = false;

    public delegate void TargetItemDelegate(Item item);
    public event TargetItemDelegate OnTargetItem;

    public delegate void OwnerMessageDelegate(TalkMessage message);
    public event OwnerMessageDelegate OnOwnerMessage;

    private Color activeTransparency = new Color(1f, 1f, 1f, 1f);
    private Color inactiveTransparency = new Color(1f, 1f, 1f, 0.3f);

    public void SetOrderItem(Item item)
    {
        if (item == null) return;
        currentItem = item;
        orderitemImage.sprite = item.Base.Sprite;
        orderitemImage.color = inactiveTransparency;
        // UIの更新などの処理をここに追加
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemBlock droppedItemBlock = eventData.pointerDrag?.GetComponent<ItemBlock>();
        if (droppedItemBlock.Item.Base == currentItem.Base)
        {
            // 既に装備中のアイテムがある場合は、バックに戻す
            OwnerMessage(new TalkMessage(MessageType.Other, MessagePanelType.Surprise, "それじゃ！"));
        }
        else
        {
            OwnerMessage(new TalkMessage(MessageType.Other, MessagePanelType.Default, "これは違うのう"));
        }
    }

    private void ClearOrderItem()
    {
        orderitemImage.sprite = null;
    }

    public void TargetItem()
    {
        OnTargetItem?.Invoke(currentItem);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TargetItem();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnTargetItem?.Invoke(null);
    }

    public void OwnerMessage(TalkMessage message)
    {
        OnOwnerMessage?.Invoke(message);
    }
}
