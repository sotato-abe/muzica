using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PocketWindow : MonoBehaviour, IDropHandler
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
    public int adjustmentBlockCount = 0; // 調整用のブロック数

    private const int MAX_PPCKET_COUNT = 20;
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

        if (droppedItemBlock != null && droppedItemBlock.Item != null)
        {
            Item item = droppedItemBlock.Item;
            if (droppedItemBlock.OriginalParent == this.transform)
                return;
            if (item is Equipment || item is Treasure)
            {
                Debug.LogWarning("装備品や宝物はポケットにドロップできません。");
                return;
            }
            playerController.AddItemToPocket(droppedItemBlock.Item);
            droppedItemBlock.RemoveItem();
            SetItems();
        }
        else
        {
            Debug.LogWarning("ドロップされたアイテムが無効です。");
        }
    }

    public void SetItems()
    {
        List<Consumable> items = playerController.PlayerCharacter.PocketList;

        foreach (Item item in items)
        {
            if (itemBlockMap.ContainsKey(item))
            {
                // 既に表示済みならスキップ
                continue;
            }

            // 新規アイテムだけ生成
            ItemBlock itemBlock = Instantiate(itemBlockPrefab, itemList.transform);
            itemBlock.Setup(item, this.transform);
            itemBlock.isOwned = true; // 所有フラグを設定
            itemBlock.OnRemoveItem += RemoveItem;
            itemBlock.OnTargetItem += TargetItem;
            itemBlockMap[item] = itemBlock;
        }

        SetCounter();
    }

    private void SetCounter()
    {
        counterText.text = $"{itemBlockMap.Count} / {playerController.PlayerCharacter.ColPocket}";
    }

    private void SetBlock()
    {
        int newBlockCount = MAX_PPCKET_COUNT - playerController.PlayerCharacter.ColPocket + adjustmentBlockCount;
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
        playerController.RemoveItemFromPocket(itemBlock.Item);
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
