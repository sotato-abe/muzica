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
    [SerializeField] int maxCount = 20;
    
    Dictionary<Item, ItemBlock> itemBlockMap = new Dictionary<Item, ItemBlock>();
    private int currentBlockCount = 0;
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

        if (droppedItemBlock != null && droppedItemBlock.Item != null && droppedItemBlock.OriginalParent != this.transform)
        {
            if (droppedItemBlock.Item is Equipment || droppedItemBlock.Item is Treasure)
            {
                Debug.LogWarning("装備品や宝物はポケットにドロップできません。");
                return;
            }
            PlayerController.Instance.AddItemToPocket(droppedItemBlock.Item);
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
        List<Consumable> items = PlayerController.Instance.PlayerCharacter.PocketList;

        // PocketListにないアイテムは削除する
        foreach (var item in new List<Item>(itemBlockMap.Keys))
        {
            if (item is Consumable consumableItem)
            {
                if (!items.Contains(consumableItem))
                {
                    if (itemBlockMap.ContainsKey(item))
                    {
                        RemoveItem(itemBlockMap[item]);
                    }
                }
            }
        }

        foreach (Item item in items)
        {
            if (itemBlockMap.ContainsKey(item))
            {
                itemBlockMap[item].SetStatusText();
                continue;
            }

            // 新規アイテムだけ生成
            ItemBlock itemBlock = Instantiate(itemBlockPrefab, itemList.transform);
            itemBlock.Setup(item, this.transform);
            itemBlock.isOwned = true; // 所有フラグを設定
            itemBlock.OnRemoveItem += RemoveItem;
            itemBlockMap[item] = itemBlock;
        }

        SetCounter();
    }

    private void SetCounter()
    {
        counterText.text = $"{itemBlockMap.Count} / {PlayerController.Instance.PlayerCharacter.ColPocket}";
    }

    private void SetBlock()
    {
        int newBlockCount = maxCount - PlayerController.Instance.PlayerCharacter.ColPocket;
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
        PlayerController.Instance.RemoveItemFromPocket(itemBlock.Item);
        itemBlock.RemovePlaceholder();
        itemBlockMap.Remove(item);
        Destroy(itemBlock.gameObject);
        SetItems();
        return true;
    }
}
