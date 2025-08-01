using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Character/CharacterBase")]
public class CharacterBase : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] RarityType rarity = RarityType.Common;
    [SerializeField] Sprite sprite;
    [SerializeField] Sprite squareSprite;
    [SerializeField, TextArea] string description;
    [SerializeField] FieldData birthplace;

    // キャラクターのエナジー
    [SerializeField] int maxLife = 10;
    [SerializeField] int maxBattery = 5;

    // キャラクターのステータス
    [SerializeField] int power = 1;
    [SerializeField] int defense = 1;
    [SerializeField] int technique = 1;
    [SerializeField] int speed = 1;
    [SerializeField] int luck = 1;

    // キャラクターの容量
    [SerializeField] int memory = 3;
    [SerializeField] int storage = 10;
    [SerializeField] int pocket = 5;
    [SerializeField] int bag = 10;

    // キャラクターの資産
    [SerializeField] int coin = 10;
    [SerializeField] int disc = 0;
    [SerializeField] int key = 0;
    [SerializeField] int exp = 0;

    // キャラクターの所持品
    [SerializeField] List<Equipment> equipmentList;
    [SerializeField] List<Consumable> pocketList;
    [SerializeField] List<Consumable> bagConsumableList;
    [SerializeField] List<Equipment> bagEquipmentList;
    [SerializeField] List<Treasure> bagTreasureList;
    [SerializeField] List<Command> storageList;
    [SerializeField] List<Command> tableList = new List<Command>(15);

    // キャラクターの会話メッセージ
    [SerializeField] List<TalkMessage> messageList;

    public string Name { get => name; }
    public RarityType Rarity { get => rarity; }
    public Sprite Sprite { get => sprite; }
    public Sprite SquareSprite { get => squareSprite; }
    public string Description { get => description; }
    public FieldData Birthplace { get => birthplace; }

    public int MaxLife { get => maxLife; }
    public int MaxBattery { get => maxBattery; }

    public int Power { get => power; }
    public int Technique { get => technique; }
    public int Defense { get => defense; }
    public int Speed { get => speed; }
    public int Luck { get => luck; }

    public int Memory { get => memory; }
    public int Storage { get => storage; }
    public int Pocket { get => pocket; }
    public int Bag { get => bag; }

    public int Coin { get => coin; }
    public int Disc { get => disc; }
    public int Key { get => key; }
    public int Exp { get => exp; }

    public List<Equipment> EquipmentList { get => equipmentList; }
    public List<Consumable> PocketList { get => pocketList; }
    public List<Consumable> BagConsumableList { get => bagConsumableList; }
    public List<Equipment> BagEquipmentList { get => bagEquipmentList; }
    public List<Treasure> BagTreasureList { get => bagTreasureList; }

    public List<Command> StorageList { get => storageList; }
    public List<Command> TableList { get => tableList; }

    public List<TalkMessage> MessageList { get => messageList; }
}
