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

    private const int MAX_BAG_COUNT = 20; // スムージングの反復回数
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
            if (itemBlockMap.ContainsKey(item))
                return;
            playerController.AddItemToBag(droppedItemBlock.Item);
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
        List<Item> items = playerController.PlayerCharacter.BagItemList;

        foreach (Item item in items)
        {
            if (itemBlockMap.ContainsKey(item))
            {
                // 既に表示済みならスキップ
                continue;
            }

            // 新規アイテムだけ生成
            ItemBlock itemBlock = Instantiate(itemBlockPrefab, itemList.transform);
            itemBlock.Setup(item);
            itemBlock.SetStatustext("New");
            itemBlock.OnRemoveItem += RemoveItem;
            itemBlockMap[item] = itemBlock;
        }

        SetCounter();
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

    private void RemoveItem(ItemBlock itemBlock)
    {
        if (itemBlock == null || itemBlock.Item == null) return;

        Item item = itemBlock.Item;
        if (itemBlockMap.ContainsKey(item))
        {
            playerController.RemoveItemFromBag(itemBlock.Item);
            itemBlock.RemovePlaceholder();
            itemBlockMap.Remove(item);
            Destroy(itemBlock.gameObject);
            SetItems();   
        }
    }

    public void ArrengeItemBlocks()
    {
        GridLayoutGroup grid = itemList.GetComponent<GridLayoutGroup>();
        if (grid != null)
        {
            grid.enabled = false;
            grid.enabled = true;
        }
    }
}
