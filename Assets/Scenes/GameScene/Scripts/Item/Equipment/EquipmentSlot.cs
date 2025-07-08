using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : EquipmentDetail, IDropHandler
{
    public UnityAction OnEquipAction;

    ItemBlock currentBlock;

    public override void SetEquipmentBlock(Equipment equipment)
    {
        foreach (Transform child in blockSlot.transform)
        {
            Destroy(child.gameObject);
        }
        // 装備アイテムのブロックを設定
        ItemBlock itemBlock = Instantiate(itemBlockPrefab, blockSlot.transform);
        itemBlock.OnRemoveItem += RemoveItem;
        itemBlock.Setup(equipment, this.transform);
        currentBlock = itemBlock;
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        ItemBlock droppedItemBlock = eventData.pointerDrag?.GetComponent<ItemBlock>();
        if (droppedItemBlock.Item is Equipment equipment)
        {
            if (currentBlock != null && currentBlock.Item != null)
            {
                // 既に装備中のアイテムがある場合は、バックに戻す
                PlayerController.Instance.AddItemToBag(currentBlock.Item);
            }
            droppedItemBlock.RemoveItem();
            PlayerController.Instance.AddItemToEquip(equipment);
            SetEquipment(equipment);
        }
    }

    public void RemoveItem(ItemBlock itemBlock)
    {
        if (itemBlock == null || itemBlock.Item == null) return;

        Item item = itemBlock.Item;
        PlayerController.Instance.RemoveItemFromEquip(item);
        itemBlock.RemovePlaceholder();
        Destroy(itemBlock.gameObject);
        currentBlock = null;
        ResetSlot();
    }
}
