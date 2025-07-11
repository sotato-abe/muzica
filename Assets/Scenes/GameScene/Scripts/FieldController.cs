using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEngine.Events;

public class FieldController : MonoBehaviour
{
    public delegate void PointEnterDelegate(PointBase point);
    public event PointEnterDelegate OnPointEnter;
    private const int ITEM_GET_PROBABILITY = 30;
    public static FieldController Instance { get; private set; }

    [SerializeField] MessagePanel messagePanel;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] DropItem dropItemPrefab; // ドロップアイテムのプレハブ
    [SerializeField] DropCommand dropCommandPrefab; // ドロップアイテムのプレハブ
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

    public void EnterPoint(PointBase point)
    {
        if (point == null)
        {
            Debug.LogWarning("Point is null. Cannot enter point.");
            return;
        }

        // メッセージパネルにメッセージを表示
        messagePanel.AddMessage(MessageIconType.Field, $"{point.Name} にアクセス。");
        OnPointEnter?.Invoke(point); // リザーブイベントを発火
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
    }

    public void DropPlayerCommand(Command command)
    {
        if (command == null) return;

        // ドロップコマンドを生成し、フィールド上にドロップする
        Vector2 position = fieldPlayer.transform.position;
        DropCommand(command, position);
    }

    public void DropItem(Item item, Vector2 centerPosition)
    {
        if (item == null) return;
        Vector2 dropPosition = FindDropPosition(centerPosition);
        DropItem dropItem = Instantiate(dropItemPrefab, dropPosition, Quaternion.identity);
        dropItem.Setup(item);
        StartCoroutine(dropItem.JumpMoveMotion(dropPosition));
    }

    public void DropCommand(Command command, Vector2 centerPosition)
    {
        if (command == null) return;

        Vector2 dropPosition = FindDropPosition(centerPosition);
        DropCommand dropCommand = Instantiate(dropCommandPrefab, dropPosition, Quaternion.identity);
        dropCommand.Setup(command);
        StartCoroutine(dropCommand.JumpMoveMotion(dropPosition));
    }

    private Vector2 FindDropPosition(Vector2 centerPosition)
    {
        const int maxTries = 50;
        float dropRadius = 1.0f;

        for (int i = 0; i < maxTries; i++)
        {
            // ランダムなオフセットを生成（中心から最大半径内）
            Vector2 offset = new Vector2(
                Random.Range(-dropRadius, dropRadius),
                Random.Range(-dropRadius, dropRadius)
            );
            Vector2 tryPosition = centerPosition + offset;

            // タイルの存在チェック
            Vector3Int cellPos = tilemap.WorldToCell(tryPosition);
            if (tilemap.HasTile(cellPos))
            {
                return tryPosition;
            }
        }

        // 落とせる場所が見つからなかった場合は中心位置を返す
        return centerPosition;
    }
}
