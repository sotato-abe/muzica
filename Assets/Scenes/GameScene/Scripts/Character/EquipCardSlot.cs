using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class EquipCardSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] EquipmentCard equipmentCard;
    [SerializeField] GameObject blockSlot;
    [SerializeField] ItemBlock itemBlockPrefab;
    [SerializeField] int equipIndex;

    public void OnDrop(PointerEventData eventData)
    {
        ItemBlock droppedItemBlock = eventData.pointerDrag?.GetComponent<ItemBlock>();
        if (droppedItemBlock.OriginalParent == this.transform) return;
        if (droppedItemBlock.Item is Equipment equipment)
        {
            PlayerController.Instance.SetEquip(equipment, equipIndex);
            droppedItemBlock.RemoveItem();
            SetEquip(equipment);
        }
    }

    public void SetEquip(Equipment equipment)
    {
        if (equipment == null)
        {
            ClearEquip();
            return;
        }
        equipmentCard.SetCard(equipment);
        equipmentCard.gameObject.SetActive(true);
        SetEquipmentBlock(equipment);
    }

    private void SetEquipmentBlock(Equipment equipment)
    {
        foreach (Transform child in blockSlot.transform)
        {
            Destroy(child.gameObject);
        }
        ItemBlock itemBlock = Instantiate(itemBlockPrefab, blockSlot.transform);
        itemBlock.Setup(equipment, this.transform);
        itemBlock.OnRemoveItem += RemoveItem;
    }

    private bool RemoveItem(ItemBlock itemBlock)
    {
        if (itemBlock == null || itemBlock.Item == null) return false;

        Item item = itemBlock.Item;
        PlayerController.Instance.RemoveEquip(equipIndex);
        Destroy(itemBlock.gameObject);
        ClearEquip();
        return true;
    }

    public void ClearEquip()
    {
        equipmentCard.gameObject.SetActive(false);
        foreach (Transform child in blockSlot.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
