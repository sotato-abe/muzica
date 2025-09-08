using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO CharacterのライフとかをEnergyに変換
[System.Serializable]
public class Character
{
    [SerializeField] protected CharacterBase _base;
    public CharacterBase Base => _base;

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
    public List<Ability> AbilityList { get; set; }
    public Equipment RightHandEquipment { get; set; }
    public Equipment LeftHandEquipment { get; set; }
    public List<Equipment> EquipmentList { get; set; }
    public List<Consumable> PocketList { get; set; }
    public List<Command> StorageList { get; set; }
    public List<Command> SlotList { get; set; }
    public List<Item> BagItemList { get; set; }

    public Character(CharacterBase baseData)
    {
        _base = baseData;
        Init();
    }

    public virtual void Init()
    {
        if (_base == null)
        {
            Debug.LogError("SetBaseStatus() failed: _base is null");
            return;
        }
        SetBaseAbility();
        SetBaseStatus();
        SetBaseItems();
        SetBaseCommands();
        ColStatus();
    }

    private void SetBaseAbility()
    {
        AbilityList = new List<Ability>();
        foreach (AbilityBase abilityBase in _base.AbilityList)
        {
            Ability ability = new Ability(abilityBase);
            AbilityList.Add(ability);
        }
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

        Level = 0;
        Exp = _base.Exp;

        Life = MaxLife;
        Battery = MaxBattery;
        Soul = 0;
    }

    private void SetBaseItems()
    {
        RightHandEquipment = _base.RightHandEquipmentBase != null ? new Equipment(_base.RightHandEquipmentBase) : null;
        LeftHandEquipment = _base.LeftHandEquipmentBase != null ? new Equipment(_base.LeftHandEquipmentBase) : null;

        EquipmentList = new List<Equipment>();
        if (RightHandEquipment != null)
        {
            EquipmentList.Add(RightHandEquipment);
        }
        if (LeftHandEquipment != null)
        {
            EquipmentList.Add(LeftHandEquipment);
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
            SlotList.Add(command);
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
            int colEnergy = (int)(energyAttack.val * energyAttack.times) * isRecovery;
            switch (energyAttack.type)
            {
                case EnergyType.Life:
                    if (energyAttack.isRecovery)
                        Life = Mathf.Clamp(Life + (int)(energyAttack.val * energyAttack.times), 0, MaxLife);
                    else
                    {
                        colEnergy = Mathf.Max(0, (int)(energyAttack.val * energyAttack.times) - ColDefense);
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
            int increaseAmount = type.GetStatusIncreaseAmount();
            switch (type)
            {
                case StatusType.LIFE:
                    MaxLife += increaseAmount;
                    break;
                case StatusType.BTRY:
                    MaxBattery += increaseAmount;
                    break;
                case StatusType.POW:
                    Power += increaseAmount;
                    break;
                case StatusType.TEC:
                    Technique += increaseAmount;
                    break;
                case StatusType.DEF:
                    Defense += increaseAmount;
                    break;
                case StatusType.SPD:
                    Speed += increaseAmount;
                    break;
                case StatusType.LUK:
                    Luck += increaseAmount;
                    break;
                case StatusType.MMR:
                    Memory += increaseAmount;
                    break;
                case StatusType.STG:
                    Storage += increaseAmount;
                    break;
                case StatusType.POC:
                    Pocket += increaseAmount;
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
