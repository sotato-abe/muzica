using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InventoryWindow : MonoBehaviour, IDropHandler
{
    [SerializeField] ItemBlock itemBlockPrefab;
    [SerializeField] GameObject refusalBlockPrefab;
    [SerializeField] GameObject itemList;
    [SerializeField] GameObject blockList;
    [SerializeField] TextMeshProUGUI counterText;
    [SerializeField] int maxCount = 20;
    
    Dictionary<Item, ItemBlock> itemBlockMap = new Dictionary<Item, ItemBlock>();
    public delegate void TargetItemDelegate(Item? item, bool isOwn = true);
    public event TargetItemDelegate OnTargetItem;

    private void Awake()
    {
        DeleteAllItems();
    }
    private void OnEnable()
    {
        SetItems();
        SetBlock();
    }

    public void OnDrop(PointerEventData eventData)
    {
        // ドロップアイテムをバックに追加
        ItemBlock droppedItemBlock = eventData.pointerDrag?.GetComponent<ItemBlock>();
        if (droppedItemBlock == null || droppedItemBlock.Item == null) return;
        if (droppedItemBlock.OriginalParent == this.transform) return;

        Item item = droppedItemBlock.Item;
        bool isRemoved = droppedItemBlock.RemoveItem();
        if (isRemoved)
        {
            PlayerController.Instance.AddItemToBag(item);
            CreateItemBlock(item, null);
            SetCounter();
        }
    }

    public void SetItems()
    {
        List<Item> items = PlayerController.Instance.PlayerCharacter.BagItemList;

        // PocketListにないアイテムは削除する
        foreach (var item in new List<Item>(itemBlockMap.Keys))
        {
            if (!items.Contains(item))
            {
                RemoveItem(itemBlockMap[item]);
            }
        }

        foreach (Item item in items)
        {
            if (itemBlockMap.ContainsKey(item))
            {
                // 既に表示済みならスキップ
                continue;
            }

            // 新規アイテムだけ生成
            CreateItemBlock(item, "New");
        }

        SetCounter();
    }

    private void CreateItemBlock(Item item, string? statusText)
    {
        if (itemBlockMap.ContainsKey(item))
        {
            // 既に表示済みならスキップ
            return;
        }

        ItemBlock itemBlock = Instantiate(itemBlockPrefab, itemList.transform);
        itemBlock.Setup(item, this.transform);
        itemBlock.SetStatustext(statusText);
        itemBlock.isOwned = true; // 所有フラグを設定
        itemBlock.OnRemoveItem += RemoveItem;
        itemBlock.OnTargetItem += TargetItem;
        itemBlockMap[item] = itemBlock;
    }

    private void SetCounter()
    {
        counterText.text = $"{itemBlockMap.Count} / {PlayerController.Instance.PlayerCharacter.Bag}";
    }

    private void SetBlock()
    {
        int newBlockCount = maxCount - PlayerController.Instance.PlayerCharacter.Bag;
        foreach (Transform child in blockList.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < newBlockCount; i++)
        {
            Instantiate(refusalBlockPrefab, blockList.transform);
        }
    }

    private void DeleteAllItems()
    {
        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var itemBlock in itemBlockMap.Values)
        {
            Destroy(itemBlock.gameObject);
        }
        itemBlockMap.Clear();
        SetCounter();
    }

    private bool RemoveItem(ItemBlock itemBlock)
    {
        if (itemBlock == null || itemBlock.Item == null) return false;
        if (itemBlock.OriginalParent != this.transform) return false;

        Item item = itemBlock.Item;
        PlayerController.Instance.RemoveItemFromBag(itemBlock.Item);
        itemBlock.RemovePlaceholder();
        itemBlockMap.Remove(item);
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
