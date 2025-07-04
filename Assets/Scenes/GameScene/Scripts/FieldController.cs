using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

public class FieldController : MonoBehaviour
{
    private const int ITEM_GET_PROBABILITY = 30;
    public static FieldController Instance { get; private set; }

    [SerializeField] MessagePanel messagePanel;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] DropItem dropItemPrefab; // ドロップアイテムのプレハブ
    [SerializeField] FieldPlayer fieldPlayer; // プレイヤーコントローラー

    private FieldData currentFieldData;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 重複防止
            return;
        }
        Instance = this;
    }

    public void SetField(FieldData fieldData)
    {
        currentFieldData = fieldData;
    }

    public void OpenTreasureBox()
    {
        if (Random.Range(0, 100f) < ITEM_GET_PROBABILITY)
        {
            messagePanel.AddMessage(MessageIconType.Treasure, $"宝ばこはカラだった。");
            return;
        }

        if (currentFieldData == null || currentFieldData.TreasureBoxItems.Count == 0)
        {
            messagePanel.AddMessage(MessageIconType.Treasure, $"宝ばこはカラだった。");
            return;
        }

        // 1. 総確率を計算
        double totalWeight = currentFieldData.TreasureBoxItems.Sum(item => item.Rarity.GetProbability());

        // 2. ランダム値を生成（0〜totalWeight）
        double randomValue = Random.Range(0f, (float)totalWeight);

        // 3. 重みでアイテムを抽選
        double cumulative = 0;
        foreach (var item in currentFieldData.TreasureBoxItems)
        {
            cumulative += item.Rarity.GetProbability();
            if (randomValue <= cumulative)
            {
                // ここでインベントリに追加する処理を入れればOK
                messagePanel.AddMessage(MessageIconType.Treasure, $"{item.Name}を手に入れた。 ({item.Rarity})");
                // item newItem = new 

                Item newItem;

                if (item is EquipmentBase equipment)
                {
                    newItem = new Equipment(equipment);
                }
                else if (item is ConsumableBase consumable)
                {
                    newItem = new Consumable(consumable);
                }
                else if (item is TreasureBase treasure)
                {
                    newItem = new Treasure(treasure);
                }
                else
                {
                    Debug.LogWarning("未対応のItemBase型：" + item.GetType().Name);
                    return;
                }
                PlayerController.Instance.AddItemToBag(newItem);
                return;
            }
        }

        // 予備処理（念のため）
        Debug.LogWarning("抽選失敗（このメッセージは基本出ない）");
    }

    public void DropPlayerItem(Item item)
    {
        if (item == null) return;

        // ドロップアイテムを生成し、フィールド上にドロップする
        Vector2 position = fieldPlayer.transform.position;
        DropItem(item, position);
        // DropItem dropItem = Instantiate(dropItemPrefab, position, Quaternion.identity);
    }

    public void DropItem(Item item, Vector2 position)
    {
        Debug.Log($"DropItem: {item?.Base.Name} at {position}");
        if (item == null) return;

        // ドロップアイテムを生成し、フィールド上にドロップする
        DropItem dropItem = Instantiate(dropItemPrefab, position, Quaternion.identity);
        dropItem.Setup(item);
    }
}
