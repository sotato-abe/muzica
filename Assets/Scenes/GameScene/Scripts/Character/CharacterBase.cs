using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Character/CharacterBase")]
public class CharacterBase : ScriptableObject
{
    [Header("Personal information")]
    [SerializeField] new string name;
    [SerializeField] RarityType rarity = RarityType.Common;
    [SerializeField] Sprite sprite;
    [SerializeField] Sprite squareSprite;
    [SerializeField, TextArea] string description;
    [SerializeField] FieldBase birthplace;

    [Header("Status")]
    // キャラクターのエナジー
    [SerializeField] int maxLife = 10;
    [SerializeField] int maxBattery = 5;
    [SerializeField] int power = 1;
    [SerializeField] int technique = 1;
    [SerializeField] int defense = 1;
    [SerializeField] int speed = 1;
    [SerializeField] int luck = 1;
    [SerializeField] int memory = 3;
    [SerializeField] int storage = 10;
    [SerializeField] int pocket = 5;
    [SerializeField] int bag = 10;

    [Header("Currency")]
    // キャラクターの資産
    [SerializeField] int coin = 10;
    [SerializeField] int disc = 0;
    [SerializeField] int key = 0;
    [SerializeField] int exp = 0;

    [Header("Belongings")]
    // キャラクターの所持品
    [SerializeField] List<AbilityBase> abilityList;
    [SerializeField] EquipmentBase rightHandEquipment;
    [SerializeField] EquipmentBase leftHandEquipment;
    [SerializeField] List<ConsumableBase> pocketBaseList;
    [SerializeField] List<ItemBase> bagItemBaseList;
    [SerializeField] List<CommandBase> slotBaseList = new List<CommandBase>(9);
    [SerializeField] List<CommandBase> storageBaseList;

    [Header("Messages")]
    // キャラクターの会話メッセージ
    [SerializeField] List<TalkMessage> messageList;

    public string Name { get => name; }
    public RarityType Rarity { get => rarity; }
    public Sprite Sprite { get => sprite; }
    public Sprite SquareSprite { get => squareSprite; }
    public string Description { get => description; }
    public FieldBase Birthplace { get => birthplace; }

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

    public EquipmentBase RightHandEquipmentBase { get => rightHandEquipment; }
    public EquipmentBase LeftHandEquipmentBase { get => leftHandEquipment; }
    public List<ConsumableBase> PocketBaseList { get => pocketBaseList; }
    public List<AbilityBase> AbilityList { get => abilityList; }
    public List<ItemBase> BagItemBaseList { get => bagItemBaseList; }
    public List<CommandBase> StorageBaseList { get => storageBaseList; }
    public List<CommandBase> SlotBaseList { get => slotBaseList; }

    // キャラクターの会話メッセージ
    public List<TalkMessage> MessageList { get => messageList; }

    // スロットの数を固定するためのプロパティ
    private void OnValidate()
    {
        const int FIRST_SLOT_COUNT = 9;

        // 要素数が固定未満なら null を追加して埋める
        while (slotBaseList.Count < FIRST_SLOT_COUNT)
        {
            slotBaseList.Add(null);
        }

        // 要素数が多すぎる場合は削る（必要なければ省略可能）
        while (slotBaseList.Count > FIRST_SLOT_COUNT)
        {
            slotBaseList.RemoveAt(slotBaseList.Count - 1);
        }
    }

}
