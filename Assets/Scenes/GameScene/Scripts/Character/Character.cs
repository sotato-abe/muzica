using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO CharacterのライフとかをEnergyに変換
[System.Serializable]
public class Character
{
    [SerializeField] CharacterBase _base;
    public CharacterBase Base { get => _base; }

    // CharacterStatus
    public int MaxLife { get; set; }
    public int Life { get; set; }
    public int MaxBattery { get; set; }
    public int Battery { get; set; }
    public int Soul { get; set; }
    public int Power { get; set; }
    public int Technique { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public int Luck { get; set; }
    public int Memory { get; set; }
    public int Storage { get; set; }
    public int Pocket { get; set; }

    // ColStatus
    public int ColLife { get; set; }
    public int ColBattery { get; set; }
    public int ColPower { get; set; }
    public int ColTechnique { get; set; }
    public int ColDefense { get; set; }
    public int ColSpeed { get; set; }
    public int ColLuck { get; set; }
    public int ColMemory { get; set; }
    public int ColStorage { get; set; }
    public int ColPocket { get; set; }
    public int Bag { get; set; }

    // Currency
    public int Coin { get; set; }
    public int Disc { get; set; }
    public int Key { get; set; }
    public int Exp { get; set; }
    public int Level { get; set; }
    public int SkillPoint { get; set; }

    // Belongings
    public List<Equipment> EquipmentList { get; set; }
    public List<Consumable> PocketList { get; set; }
    public List<Command> StorageList { get; set; }
    public List<Command> SlotList { get; set; }
    public List<Item> BagItemList { get; set; }

    public static Character CreateFrom(CharacterBase baseData)
    {
        var character = new Character
        {
            _base = baseData,
            EquipmentList = new List<Equipment>(),
            PocketList = new List<Consumable>(),
            StorageList = new List<Command>(),
            SlotList = new List<Command>(),
            BagItemList = new List<Item>()
        };

        character.Init();
        return character;
    }

    public virtual void Init()
    {
        if (_base == null)
        {
            Debug.LogError("SetBaseStatus() failed: _base is null");
            return;
        }
        SetBaseStatus();
        SetBaseItems();
        SetBaseCommands();
        ColStatus();
    }

    private void SetBaseStatus()
    {
        MaxLife = _base.MaxLife;
        MaxBattery = _base.MaxBattery;
        Power = _base.Power;
        Technique = _base.Technique;
        Defense = _base.Defense;
        Speed = _base.Speed;
        Luck = _base.Luck;
        Memory = _base.Memory;
        Storage = _base.Storage;
        Pocket = _base.Pocket;
        Bag = _base.Bag;

        Coin = _base.Coin;
        Disc = _base.Disc;
        Key = _base.Key;

        Level = 1;
        Exp = _base.Exp;

        Life = MaxLife;
        Battery = MaxBattery;
        Soul = 0;
    }

    private void SetBaseItems()
    {
        EquipmentList = new List<Equipment>();
        foreach (EquipmentBase equipmentBase in _base.EquipmentBaseList)
        {
            Equipment equipment = new Equipment(equipmentBase);
            EquipmentList.Add(equipment);
        }

        PocketList = new List<Consumable>();
        foreach (ConsumableBase consumableBase in _base.PocketBaseList)
        {
            Consumable consumable = new Consumable(consumableBase);
            PocketList.Add(consumable);
        }

        BagItemList = new List<Item>();
        foreach (ItemBase item in _base.BagItemBaseList)
        {
            switch (item.itemType)
            {
                case ItemType.Consumable:
                    Consumable bagConsumable = new Consumable((ConsumableBase)item);
                    BagItemList.Add(bagConsumable);
                    break;
                case ItemType.Equipment:
                    Equipment bagEquipment = new Equipment((EquipmentBase)item);
                    BagItemList.Add(bagEquipment);
                    break;
                case ItemType.Treasure:
                    Treasure bagTreasure = new Treasure((TreasureBase)item);
                    BagItemList.Add(bagTreasure);
                    break;
                default:
                    Debug.LogError("Unknown item type: " + item.itemType);
                    break;
            }
        }
    }

    private void SetBaseCommands()
    {
        SlotList = new List<Command>();
        foreach (CommandBase commandBase in _base.SlotBaseList)
        {
            Command command = new Command(commandBase);
            SlotList.Add(command); // 追加する方法ならエラーにならない
        }

        StorageList = new List<Command>();
        foreach (CommandBase commandBase in _base.StorageBaseList)
        {
            Command command = new Command(commandBase);
            StorageList.Add(command);
        }
    }

    public void ColStatus()
    {
        int DiffLife = 0;
        int DiffBattery = 0;
        int DiffPower = 0;
        int DiffTechnique = 0;
        int DiffDefense = 0;
        int DiffSpeed = 0;
        int DiffLuck = 0;
        int DiffMemory = 0;
        int DiffStorage = 0;
        int DiffPocket = 0;

        foreach (Equipment equipment in EquipmentList)
        {
            DiffLife += equipment.EquipmentBase.Life;
            DiffBattery += equipment.EquipmentBase.Battery;
            DiffPower += equipment.EquipmentBase.Power;
            DiffTechnique += equipment.EquipmentBase.Technique;
            DiffDefense += equipment.EquipmentBase.Defense;
            DiffSpeed += equipment.EquipmentBase.Speed;
            DiffLuck += equipment.EquipmentBase.Luck;
        }

        ColLife = MaxLife + DiffLife;
        Life = Mathf.Min(Life, ColLife); // LifeがMaxLifeを超えないようにする
        ColBattery = MaxBattery + DiffBattery;
        Battery = Mathf.Min(Battery, ColBattery); // BatteryがMaxBatteryを超えないようにする
        ColPower = Power + DiffPower;
        ColTechnique = Technique + DiffTechnique;
        ColDefense = Defense + DiffDefense;
        ColSpeed = Speed + DiffSpeed;
        ColLuck = Luck + DiffLuck;
        ColMemory = Memory + DiffMemory;
        ColStorage = Storage + DiffStorage;
        ColPocket = Pocket + DiffPocket;
    }

    public void TakeAttack(TotalAttackCount totalCount)
    {
        List<EnergyCount> energyAttackList = totalCount.EnergyAttackList;
        foreach (var energyAttack in energyAttackList)
        {
            // エネルギー攻撃の処理
            int isRecovery = energyAttack.isRecovery ? -1 : 1;
            int colEnergy = energyAttack.val * energyAttack.times * isRecovery;
            switch (energyAttack.type)
            {
                case EnergyType.Life:
                    if (energyAttack.isRecovery)
                        Life = Mathf.Clamp(Life + (energyAttack.val * energyAttack.times), 0, MaxLife);
                    else
                    {
                        colEnergy = Mathf.Max(0, (energyAttack.val - ColDefense) * energyAttack.times);
                        Life = Mathf.Clamp(Life - colEnergy, 0, MaxLife);
                    }
                    break;
                case EnergyType.Battery:
                    Battery = Mathf.Clamp(Battery - colEnergy, 0, MaxBattery);
                    break;
                case EnergyType.Soul:
                    Soul = Mathf.Clamp(Soul - colEnergy, 0, 100);
                    break;
                default:
                    Debug.LogWarning($"Unknown energy type: {energyAttack.type}");
                    break;
            }

        }
    }

    public void StatusUp(StatusType type)
    {
        if (SkillPoint > 0)
        {
            switch (type)
            {
                case StatusType.LIFE:
                    MaxLife += 10;
                    break;
                case StatusType.BTRY:
                    MaxBattery += 10;
                    break;
                case StatusType.POW:
                    Power += 1;
                    break;
                case StatusType.TEC:
                    Technique += 1;
                    break;
                case StatusType.DEF:
                    Defense += 1;
                    break;
                case StatusType.SPD:
                    Speed += 1;
                    break;
                case StatusType.LUK:
                    Luck += 1;
                    break;
                case StatusType.MMR:
                    Memory += 1;
                    break;
                case StatusType.STG:
                    Storage += 1;
                    break;
                case StatusType.POC:
                    Pocket += 1;
                    break;
            }
            SkillPoint -= 1;
            ColStatus();
        }
        else
        {
            Debug.Log("スキルポイントが足りません。");
        }
    }

    public TalkMessage GetTalkMessageByType(MessageType messageType)
    {
        // Base.MessageListからmessageTypeに一致するメッセージを取得
        List<TalkMessage> messages = Base.MessageList.FindAll(m => m.messageType == messageType);
        if (messages.Count > 0)
        {
            // ランダムにメッセージを選択
            int randomIndex = Random.Range(0, messages.Count);
            TalkMessage selectedMessage = messages[randomIndex];
            return selectedMessage;
        }
        else
        {
            return TalkMessage.GetDefaultMessage(messageType);
        }
    }
}
