using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OrderItemSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image orderitemImage;
    [SerializeField] public ItemBlock itemBlockPrefab;
    [SerializeField] GameObject blockSlot;

    public Item currentItem;
    private bool isSet = false;
    public bool IsSet { get { return isSet; } }

    public delegate void OwnerMessageDelegate(TalkMessage message);
    public event OwnerMessageDelegate OnOwnerMessage;

    public delegate void SetItemDelegate();
    public event SetItemDelegate OnSetItem;

    private Color activeTransparency = new Color(1f, 1f, 1f, 1f);
    private Color inactiveTransparency = new Color(1f, 1f, 1f, 0.3f);

    public void SetOrderItem(Item item)
    {
        if (item == null) return;
        currentItem = item;
        orderitemImage.sprite = item.Base.Sprite;
        orderitemImage.color = inactiveTransparency;
        ClearSlotBlock();
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemBlock droppedItemBlock = eventData.pointerDrag?.GetComponent<ItemBlock>();
        if (isSet) return;
        if (droppedItemBlock.Item.Base == currentItem.Base)
        {
            OwnerMessage(new TalkMessage(MessageType.Other, MessagePanelType.Surprise, "これじゃ！"));
            droppedItemBlock.Hide();
            currentItem = droppedItemBlock.Item;
            ItemBlock itemBlock = Instantiate(itemBlockPrefab, blockSlot.transform);
            itemBlock.Setup(droppedItemBlock.Item, this.transform);
            isSet = true;
            OnSetItem?.Invoke();
        }
        else
        {
            OwnerMessage(new TalkMessage(MessageType.Other, MessagePanelType.Default, "これは違うのう"));
            isSet = false;
        }
    }

    private void ClearOrderItem()
    {
        orderitemImage.sprite = null;
    }

    private void ClearSlotBlock()
    {
        foreach (Transform child in blockSlot.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipUI.Instance.TargetItem(currentItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.ClearTargetItem();
    }

    public void OwnerMessage(TalkMessage message)
    {
        OnOwnerMessage?.Invoke(message);
    }
}
