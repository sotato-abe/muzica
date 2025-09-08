using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : EquipmentDetail, IDropHandler
{
    public UnityAction OnUpdateInventory;
    public delegate void TargetItemDelegate(Item item, bool isOwn = true);
    public event TargetItemDelegate OnTargetItem;
    ItemBlock currentBlock;
    public BodyPartType bodyPartType = BodyPartType.None;

    public override void SetEquipmentBlock(Equipment equipment)
    {
        foreach (Transform child in blockSlot.transform)
        {
            Destroy(child.gameObject);
        }
        // 装備アイテムのブロックを設定
        ItemBlock itemBlock = Instantiate(itemBlockPrefab, blockSlot.transform);
        itemBlock.OnRemoveItem += RemoveItem;
        itemBlock.OnTargetItem += TargetItem;
        itemBlock.Setup(equipment, this.transform);
        currentBlock = itemBlock;
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemBlock droppedItemBlock = eventData.pointerDrag?.GetComponent<ItemBlock>();
        if (droppedItemBlock.Item is Equipment equipment)
        {
            // 既に装備中のアイテムがある場合は、バックに戻す
            PlayerController.Instance.SetEquipmentByBodyPart(bodyPartType, equipment);
            OnUpdateInventory?.Invoke();
            droppedItemBlock.RemoveItem();
            SetEquipment(equipment);
        }
    }

    public bool RemoveItem(ItemBlock itemBlock)
    {
        if (itemBlock == null || itemBlock.Item == null) return false;

        PlayerController.Instance.RemoveEquipmentbyBodyPart(bodyPartType);
        itemBlock.RemovePlaceholder();
        Destroy(itemBlock.gameObject);
        currentBlock = null;
        ResetSlot();
        return true;
    }

    public void TargetItem(ItemBlock itemBlock)
    {
        if (itemBlock == null || itemBlock.Item == null)
        {
            OnTargetItem?.Invoke(null);
            return;
        }
        OnTargetItem?.Invoke(itemBlock.Item);
    }
}
