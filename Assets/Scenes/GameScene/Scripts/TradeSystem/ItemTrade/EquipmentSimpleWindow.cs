using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EquipmentSimpleWindow : MonoBehaviour, IDropHandler
{
    [SerializeField] ItemBlock itemBlockPrefab;
    [SerializeField] GameObject itemList;
    PlayerController playerController;

    public delegate void TargetItemDelegate(Item? item, bool isOwn = true);
    public event TargetItemDelegate OnTargetItem;

    private int currentBlockCount = 0;
    private void Awake()
    {
        playerController = PlayerController.Instance;
    }
    private void OnEnable()
    {
        if (PlayerController.Instance == null) return;

        playerController = PlayerController.Instance;
        SetItems();
    }

    public void OnDrop(PointerEventData eventData)
    {
        // ドロップアイテムをバックに追加
        ItemBlock droppedItemBlock = eventData.pointerDrag?.GetComponent<ItemBlock>();

        if (droppedItemBlock == null && droppedItemBlock.Item == null) return;
        if (droppedItemBlock.OriginalParent == this.transform) return;

        Item item = droppedItemBlock.Item;
        if (item is Equipment)
        {
            bool isBought = droppedItemBlock.RemoveItem();
            if (isBought)
            {
                playerController.AddItemToEquip(droppedItemBlock.Item);
                SetItems();
            }
        }
    }

    public void SetItems()
    {
        List<Equipment> items = playerController.PlayerCharacter.EquipmentList;
        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in items)
        {
            // 新規アイテムだけ生成
            ItemBlock itemBlock = Instantiate(itemBlockPrefab, itemList.transform);
            itemBlock.Setup(item, this.transform);
            itemBlock.OnRemoveItem += RemoveItem;
            itemBlock.OnTargetItem += TargetItem;
        }

    }

    private bool RemoveItem(ItemBlock itemBlock)
    {
        if (itemBlock == null || itemBlock.Item == null) return false;
        if (itemBlock.OriginalParent != this.transform) return false;

        playerController.RemoveItemFromEquip(itemBlock.Item);
        itemBlock.RemovePlaceholder();
        Destroy(itemBlock.gameObject);
        SetItems();
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
