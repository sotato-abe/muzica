using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class EquipmentArea : MonoBehaviour, IDropHandler
{
    public UnityAction OnEquipAction;
    public virtual void OnDrop(PointerEventData eventData)
    {
        ItemBlock droppedItemBlock = eventData.pointerDrag?.GetComponent<ItemBlock>();
        if (droppedItemBlock.Item is Equipment equipment)
        {
            droppedItemBlock.EquipItem();
            OnEquipAction?.Invoke();
        }
    }
}
