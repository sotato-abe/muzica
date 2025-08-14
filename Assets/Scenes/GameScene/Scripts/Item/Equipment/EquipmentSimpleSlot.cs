using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSimpleSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] public GameObject blockSlot;
    [SerializeField] public ItemBlock itemBlockPrefab;
    [SerializeField] Image backIcon;
    public UnityAction OnUpdateInventory;
    public delegate void TargetItemDelegate(Item item, bool isOwn = true);
    public event TargetItemDelegate OnTargetItem;
    ItemBlock currentBlock;

    public BodyPartType bodyPartType = BodyPartType.None;

    private void OnEnable()
    {
        SetEquipmentBlock();
    }

    public void SetEquipmentBlock()
    {
        Equipment equipment = PlayerController.Instance.GetEquipmentByBodyPart(bodyPartType);
        foreach (Transform child in blockSlot.transform)
        {
            Destroy(child.gameObject);
        }
        if (equipment == null)
        {
            return;
        }
        // 装備アイテムのブロックを設定
        ItemBlock itemBlock = Instantiate(itemBlockPrefab, blockSlot.transform);
        itemBlock.OnRemoveItem += RemoveItem;
        itemBlock.OnTargetItem += TargetItem;
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
                OnUpdateInventory?.Invoke();
            }
            droppedItemBlock.RemoveItem();
            PlayerController.Instance.SetEquipmentByBodyPart(bodyPartType, equipment);
            SetEquipmentBlock();
        }
    }

    public bool RemoveItem(ItemBlock itemBlock)
    {
        if (itemBlock == null || itemBlock.Item == null) return false;

        Item item = itemBlock.Item;
        PlayerController.Instance.RemoveEquipmentbyBodyPart(bodyPartType);
        itemBlock.RemovePlaceholder();
        Destroy(itemBlock.gameObject);
        currentBlock = null;
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
