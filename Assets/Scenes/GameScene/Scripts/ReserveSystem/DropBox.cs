using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

// ItemBlockをドロップするためのドロップボックス
// TODO：commandBlockをドロップする
public class DropBox : FocusScaleUp, IDropHandler
{
    public virtual void OnDrop(PointerEventData eventData)
    {
        ItemBlock droppedItemBlock = eventData.pointerDrag?.GetComponent<ItemBlock>();
        PlayerController.Instance.DropItem(droppedItemBlock.Item);
        droppedItemBlock.RemoveItem();
    }
}
