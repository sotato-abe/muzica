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

    // battle
    public int LifeGuard { get; set; }
    public int BatteryGuard { get; set; }
    public List<Enchant> EnchantList { get; set; }

    private int DiffLife = 0;
    private int DiffBattery = 0;
    private int DiffPower = 0;
    private int DiffTechnique = 0;
    private int DiffDefense = 0;
    private int DiffSpeed = 0;
    private int DiffLuck = 0;
    private int DiffMemory = 0;
    private int DiffStorage = 0;
    private int DiffPocket = 0;

    // message
    public delegate void TalkMessageDelegate(MessageType message);

    // TODO BattleCharacter に継承させる
    public delegate void LifeGuargeDelegate(int value, int guard, int takeValue);
    public event LifeGuargeDelegate OnLifeGuargeChange;
    public delegate void BatteryGuargeDelegate(int value, int guard, int takeValue);
    public event BatteryGuargeDelegate OnBatteryGuargeChange;
    public delegate void SoulGuargeDelegate(int value, int guard, int takeValue);
    public event SoulGuargeDelegate OnSoulGuargeChange;

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
        LifeGuard = 0;
        BatteryGuard = 0;
        EnchantList = new List<Enchant>();
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
            consumable.isNew = false;
            PocketList.Add(consumable);
        }

        BagItemList = new List<Item>();
        foreach (ItemBase item in _base.BagItemBaseList)
        {
            switch (item.itemType)
            {
                case ItemType.Consumable:
                    Consumable bagConsumable = new Consumable((ConsumableBase)item);
                    bagConsumable.isNew = false;
                    BagItemList.Add(bagConsumable);
                    break;
                case ItemType.Equipment:
                    Equipment bagEquipment = new Equipment((EquipmentBase)item);
                    bagEquipment.isNew = false;
                    BagItemList.Add(bagEquipment);
                    break;
                case ItemType.Treasure:
                    Treasure bagTreasure = new Treasure((TreasureBase)item);
                    bagTreasure.isNew = false;
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
            command.isNew = false;
            SlotList.Add(command);
        }

        StorageList = new List<Command>();
        foreach (CommandBase commandBase in _base.StorageBaseList)
        {
            Command command = new Command(commandBase);
            command.isNew = false;
            StorageList.Add(command);
        }
    }

    public void ColStatus()
    {
        DiffLife = 0;
        DiffBattery = 0;
        DiffPower = 0;
        DiffTechnique = 0;
        DiffDefense = 0;
        DiffSpeed = 0;
        DiffLuck = 0;
        DiffMemory = 0;
        DiffStorage = 0;
        DiffPocket = 0;

        EnchantActivation();

        // TODO Enchantによる強化を追加
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

    private void EnchantActivation()
    {
        if (EnchantList == null || EnchantList.Count == 0) return;

        List<Enchant> PositiveEnchantList = new List<Enchant>();
        List<Enchant> NegativeEnchantList = new List<Enchant>();
        foreach (Enchant enchant in EnchantList)
        {
            if (enchant.Type.AssigneeSelf())
            {
                PositiveEnchantList.Add(enchant);
            }
            else
            {
                NegativeEnchantList.Add(enchant);
            }
        }
        if (PositiveEnchantList.Count > 0)
            PositiveEnchantActivation(PositiveEnchantList);
        if (NegativeEnchantList.Count > 0)
            NegativeEnchantActivation(NegativeEnchantList);
    }

    private void PositiveEnchantActivation(List<Enchant> PositiveEnchantList)
    {
        foreach (Enchant enchant in PositiveEnchantList)
        {
            switch (enchant.Type)
            {
                case EnchantType.Acceleration:
                    DiffSpeed += enchant.Val;
                    break;
                case EnchantType.Lucky:
                    DiffLuck += enchant.Val;
                    break;
                case EnchantType.Gaze:
                    // 敵のステータスを表示する処理
                    break;
                case EnchantType.Analysis:
                    // 敵の詳細情報を表示する処理
                    break;
                case EnchantType.Power:
                    DiffPower += enchant.Val;
                    break;
                case EnchantType.Adrenalin:
                    // アタック時に倍率を10％上げる処理
                    break;
                case EnchantType.Guard:
                    LifeGuard += enchant.Val;
                    // ガード時に基本値を１上げる処理
                    break;
                case EnchantType.Solid:
                    LifeGuard += (int)(LifeGuard * (enchant.Val * 2) / 100);
                    // ガード時に倍率を2%上げる処理
                    break;
                case EnchantType.Curing:
                    // TODO :アタックを受ける時、ダメージを半分にする
                    break;
                case EnchantType.Splinter:
                    // TODO :アタックを受ける時、攻撃者にスタック分のLIFEダメージを与える
                    break;
                case EnchantType.Reflection:
                    // TODO :アタックを受ける時、攻撃者にスタック分の
                    break;
                case EnchantType.Camouflage:
                    // TODO :アタックを受ける時、当たり判定をスタック＊
                    break;
                case EnchantType.Clear:
                    // ターン開始時に自身に付与されているデバフをランダムで一つ解除する、
                    List<Enchant> negativeEnchants = EnchantList.FindAll(e => !e.Type.AssigneeSelf());
                    int clearCount = enchant.Val;
                    if (negativeEnchants.Count == 0) break;
                    for (int i = 0; i < clearCount; i++)
                    {
                        int randomIndex = Random.Range(0, negativeEnchants.Count);
                        Enchant enchantToRemove = negativeEnchants[randomIndex];
                        EnchantList.Remove(enchantToRemove);
                    }
                    break;
                default:
                    Debug.LogWarning("未実装のポジティブエンチャントです: " + enchant.Type);
                    break;
            }
        }
    }

    private void NegativeEnchantActivation(List<Enchant> NegativeEnchantList)
    {
        foreach (Enchant enchant in NegativeEnchantList)
        {
            switch (enchant.Type)
            {
                case EnchantType.Slow:
                    // Speedを下げる処理
                    DiffSpeed -= enchant.Val;
                    break;
                case EnchantType.UnLucky:
                    // Luckを下げる処理
                    DiffLuck -= enchant.Val;
                    break;
                case EnchantType.Fatigue:
                    // SOULを回復させない処理
                    break;
                case EnchantType.Lock:
                    // TODO : ランダムでスロットが１列実行不能になる処理
                    break;
                case EnchantType.Cipher:
                    // TODO : 装備のエナジーコストが20％アップする処理
                    break;
                case EnchantType.Bug:
                    // TODO : コマンド発動時にスタック分BTRYダメージを受ける処理
                    break;
                case EnchantType.Atrophy:
                    // アタック時に基本値を１下げる処理
                    DiffPower -= enchant.Val;
                    break;
                case EnchantType.Blind:
                    // TODO : アタック時に成功確率をスタック＊２％下げる処理
                    break;
                case EnchantType.Paralysis:
                    // TODO :アタック時にターゲット指定をランダムにする処理
                    break;
                case EnchantType.Crack:
                    // ガード時に基本値を１下げる処理
                    LifeGuard -= enchant.Val;
                    break;
                case EnchantType.Poison:
                    // ターン開始時にスタック分のLIFEダメージを受ける処理
                    Life -= enchant.Val;
                    break;
                case EnchantType.Leakage:
                    // ターン開始時にスタック分のBTRYダメージを受ける処理
                    Battery -= enchant.Val;
                    break;
                case EnchantType.Hurt:
                    // ターン開始時にスタック分のLIFEダメージを受ける処理
                    Life -= enchant.Val;
                    break;
                case EnchantType.Sleep:
                    // ターン開始時にスタック分の確率で発動しターンがスキップされる処理
                    break;
                default:
                    Debug.LogWarning("未実装のネガティブエンチャントです: " + enchant.Type);
                    break;
            }
        }
    }

    public void ReduceEnchant()
    {
        List<Enchant> enchantsToRemove = new List<Enchant>();
        foreach (Enchant enchant in EnchantList)
        {
            bool stillExists = enchant.Reuduce1Enchant();
            if (!stillExists)
            {
                enchantsToRemove.Add(enchant);
            }
        }
        foreach (Enchant enchant in enchantsToRemove)
        {
            EnchantList.Remove(enchant);
        }
    }

    public void EnchantClear()
    {
        EnchantList.Clear();
    }

    public void TakeTotalAttack(TotalAttack totalAttack)
    {
        TakeEnchantList(totalAttack.EnchantList);
        TakeAttackList(totalAttack.AttackList);
    }

    public void TakeAttackList(List<Attack> attackList)
    {
        foreach (Attack attack in attackList)
        {
            switch (attack.AttackType)
            {
                case AttackType.SoloLifeUp:
                case AttackType.GroupLifeUp:
                    int takeLifeAttackValue = Mathf.FloorToInt(attack.Val * attack.Times);
                    Life += takeLifeAttackValue;
                    Life = Mathf.Min(Life, ColLife);
                    OnLifeGuargeChange?.Invoke(Life, LifeGuard, takeLifeAttackValue);
                    break;
                case AttackType.SoloBatteryUp:
                case AttackType.GroupBatteryUp:
                    int takeBatteryAttackValue = Mathf.FloorToInt(attack.Val * attack.Times);
                    Battery += takeBatteryAttackValue;
                    Battery = Mathf.Min(Battery, ColBattery);
                    OnBatteryGuargeChange?.Invoke(Battery, BatteryGuard, takeBatteryAttackValue);
                    break;
                case AttackType.SoloSoulUp:
                case AttackType.GroupSoulUp:
                    int takeSoulAttackValue = Mathf.FloorToInt(attack.Val * attack.Times);
                    Soul += takeSoulAttackValue;
                    Soul = Mathf.Min(Soul, 100);
                    OnSoulGuargeChange?.Invoke(Soul, 0, takeSoulAttackValue);
                    break;
                case AttackType.SoloLifeDown:
                case AttackType.GroupLifeDown:
                    int damage = Mathf.FloorToInt(attack.Val * attack.Times);
                    if (LifeGuard > 0)
                    {
                        if (LifeGuard >= damage)
                        {
                            LifeGuard -= damage;
                            damage = 0;
                        }
                        else
                        {
                            damage -= LifeGuard;
                            LifeGuard = 0;
                        }
                    }
                    Life -= damage;
                    Life = Mathf.Max(Life, 0);
                    OnLifeGuargeChange?.Invoke(Life, LifeGuard, -damage);
                    break;
                case AttackType.SoloBatteryDown:
                case AttackType.GroupBatteryDown:
                    int btryDamage = Mathf.FloorToInt(attack.Val * attack.Times);
                    if (BatteryGuard > 0)
                    {
                        if (BatteryGuard >= btryDamage)
                        {
                            BatteryGuard -= btryDamage;
                            btryDamage = 0;
                        }
                        else
                        {
                            btryDamage -= BatteryGuard;
                            BatteryGuard = 0;
                        }
                    }
                    Battery -= btryDamage;
                    Battery = Mathf.Max(Battery, 0);
                    OnBatteryGuargeChange?.Invoke(Battery, BatteryGuard, -btryDamage);
                    break;
                case AttackType.SoloSoulDown:
                case AttackType.GroupSoulDown:
                    int soulDamage = Mathf.FloorToInt(attack.Val * attack.Times);
                    Soul -= soulDamage;
                    Soul = Mathf.Max(Soul, 0);
                    OnSoulGuargeChange?.Invoke(Soul, 0, -soulDamage);
                    break;
                case AttackType.SoloGuard:
                case AttackType.GroupGuard:
                    int guardIncrease = Mathf.FloorToInt(attack.Val * attack.Times);
                    LifeGuard += guardIncrease;
                    OnLifeGuargeChange?.Invoke(Life, 0, guardIncrease);
                    break;
                case AttackType.SoloSecurity:
                case AttackType.GroupSecurity:
                    int batteryGuardIncrease = Mathf.FloorToInt(attack.Val * attack.Times);
                    BatteryGuard += batteryGuardIncrease;
                    OnBatteryGuargeChange?.Invoke(Battery, 0, batteryGuardIncrease);
                    break;
                default:
                    Debug.LogWarning("未実装のAttackTypeです: " + attack.AttackType);
                    break;
            }
        }
    }

    public void TakeEnchantList(List<Enchant> enchantList)
    {
        foreach (Enchant enchant in enchantList)
        {
            Enchant existingEnchant = EnchantList.Find(e => e.Type == enchant.Type);
            if (existingEnchant != null)
            {
                existingEnchant.Val += enchant.Val;
            }
            else
            {
                EnchantList.Add(enchant);
            }
        }
    }

    public void ClearEnchantList()
    {
        EnchantList.Clear();
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

    // BattleCharacterに分離したい
    public void UpdateLifeGuard(int guard)
    {
        SoundSystem.Instance.PlaySE(SeType.Guard);
        LifeGuard = Mathf.Max(0, guard);
    }

    public void UpdateBatteryGuard(int guard)
    {
        SoundSystem.Instance.PlaySE(SeType.Guard);
        BatteryGuard = Mathf.Max(0, guard);
    }
}
