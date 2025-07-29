using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEngine.Events;

public class FieldController : MonoBehaviour
{
    public delegate void PointEnterDelegate(Point point);
    public event PointEnterDelegate OnPointEnter;
    private const int ITEM_GET_PROBABILITY = 30;
    public static FieldController Instance { get; private set; }

    [SerializeField] MessagePanel messagePanel;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] DropItem dropItemPrefab; // ドロップアイテムのプレハブ
    [SerializeField] DropCommand dropCommandPrefab; // ドロップアイテムのプレハブ
    [SerializeField] FieldPlayer fieldPlayer; // プレイヤーコントローラー
    [SerializeField] List<EnemyGroup> defaultEnemyGroups = new List<EnemyGroup>();
    [SerializeField] List<EnemyGroup> desertEnemyGroups = new List<EnemyGroup>();
    [SerializeField] List<EnemyGroup> wildernessEnemyGroups = new List<EnemyGroup>();
    [SerializeField] List<EnemyGroup> grasslandsEnemyGroups = new List<EnemyGroup>();
    [SerializeField] List<EnemyGroup> wetlandsEnemyGroups = new List<EnemyGroup>();
    [SerializeField] List<EnemyGroup> snowEnemyGroups = new List<EnemyGroup>();
    [SerializeField] List<EnemyGroup> rockEnemyGroups = new List<EnemyGroup>();
    [SerializeField] List<EnemyGroup> magmaEnemyGroups = new List<EnemyGroup>();
    [SerializeField] List<EnemyGroup> pollutionEnemyGroups = new List<EnemyGroup>();
    [SerializeField] List<EnemyGroup> seaEnemyGroups = new List<EnemyGroup>();
    [SerializeField] List<EnemyGroup> oceanEnemyGroups = new List<EnemyGroup>();

    private Dictionary<FieldType, List<EnemyGroup>> fieldTypeEnemyGroups;

    private FieldData currentFieldData;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 重複防止
            return;
        }
        Instance = this;

        fieldTypeEnemyGroups = new Dictionary<FieldType, List<EnemyGroup>>
        {
            { FieldType.Default, defaultEnemyGroups },
            { FieldType.Desert, desertEnemyGroups },
            { FieldType.Wilderness, wildernessEnemyGroups },
            { FieldType.Grasslands, grasslandsEnemyGroups },
            { FieldType.Wetlands, wetlandsEnemyGroups },
            { FieldType.Snow, snowEnemyGroups },
            { FieldType.Rock, rockEnemyGroups },
            { FieldType.Magma, magmaEnemyGroups },
            { FieldType.Pollution, pollutionEnemyGroups },
            { FieldType.Sea, seaEnemyGroups },
            { FieldType.Ocean, oceanEnemyGroups }
        };
    }

    public void SetField(FieldData fieldData)
    {
        currentFieldData = fieldData;
    }

    // 複数の敵をグループから取得するように変更する
    public List<Character> GetEnemies()
    {
        if (currentFieldData == null)
        {
            Debug.LogWarning("Current field data is null. Cannot get enemies.");
            return new List<Character>();
        }

        List<Character> enemies = new List<Character>();
        EnemyGroup targetGroup = null; // ← new は使わない

        int groupCount = currentFieldData.EnemyGroups.Count;
        if (groupCount == 0)
        {
            List<EnemyGroup> targetGroups = fieldTypeEnemyGroups[currentFieldData.fieldType];
            targetGroup = targetGroups[Random.Range(0, targetGroups.Count)];
        }
        else
        {
            targetGroup = currentFieldData.EnemyGroups[Random.Range(0, groupCount)];
        }

        if (targetGroup != null)
        {
            enemies.AddRange(targetGroup.GetRandomCharacterList());
        }
        else
        {
            Debug.LogWarning("No target group found for enemies.");
        }

        return enemies;
    }

    public void EnterPoint(Point point)
    {
        if (point == null)
        {
            Debug.LogWarning("Point is null. Cannot enter point.");
            return;
        }

        // メッセージパネルにメッセージを表示
        messagePanel.AddMessage(MessageIconType.Field, $"{point.Base.Name} にアクセス。");
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
