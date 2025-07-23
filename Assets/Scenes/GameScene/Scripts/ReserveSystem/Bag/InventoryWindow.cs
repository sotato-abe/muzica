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
    PlayerController playerController;
    Dictionary<Item, ItemBlock> itemBlockMap = new Dictionary<Item, ItemBlock>();

    public delegate void TargetItemDelegate(Item? item);
    public event TargetItemDelegate OnTargetItem;

    private const int MAX_BAG_COUNT = 20;
    private int currentBlockCount = 0;
    private void Awake()
    {
        playerController = PlayerController.Instance;
        DeleteAllItems();
    }
    private void OnEnable()
    {
        if (PlayerController.Instance == null) return;

        playerController = PlayerController.Instance;
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
            playerController.AddItemToBag(item);
            CreateItemBlock(item, null);
            SetCounter();
        }
    }

    public void SetItems()
    {
        List<Item> items = playerController.PlayerCharacter.BagItemList;

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
        counterText.text = $"{itemBlockMap.Count} / {playerController.PlayerCharacter.Bag}";
    }

    private void SetBlock()
    {
        int newBlockCount = MAX_BAG_COUNT - playerController.PlayerCharacter.Bag;
        if (currentBlockCount == newBlockCount)
            return;

        currentBlockCount = newBlockCount;
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
        playerController.RemoveItemFromBag(itemBlock.Item);
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
