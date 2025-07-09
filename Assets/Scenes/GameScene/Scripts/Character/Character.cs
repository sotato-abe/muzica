using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO CharacterのライフとかをEnegyに変換
[System.Serializable]
public class Character
{
    [SerializeField] CharacterBase _base;
    public CharacterBase Base { get => _base; }

    // CharacterEnegy
    public int MaxLife { get; set; }
    public int ColLife { get; set; }
    public int Life { get; set; }
    public int MaxBattery { get; set; }
    public int ColBattery { get; set; }
    public int Battery { get; set; }
    public int Soul { get; set; }

    // CharacterStatus
    public int Level { get; set; }
    public int Power { get; set; }
    public int Technique { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public int Luck { get; set; }
    public int Memory { get; set; }
    public int Storage { get; set; }
    public int Pocket { get; set; }
    public int ColPower { get; set; }
    public int ColTechnique { get; set; }
    public int ColDefense { get; set; }
    public int ColSpeed { get; set; }
    public int ColLuck { get; set; }
    public int ColMemory { get; set; }
    public int ColStorage { get; set; }
    public int ColPocket { get; set; }
    public int Bag { get; set; }

    // Property
    public int Money { get; set; }
    public int Disk { get; set; }
    public int Key { get; set; }
    public int Exp { get; set; }

    // Inventory
    public List<Equipment> EquipmentList { get; set; }
    public List<Consumable> PocketList { get; set; }
    public List<Command> StorageList { get; set; }
    public List<Command> TableList { get; set; }
    public List<Item> BagItemList { get; set; }

    public virtual void Init()
    {
        if (_base == null)
        {
            Debug.LogError("Init() failed: _base is null");
            return;
        }

        MaxLife = _base.MaxLife;
        Life = MaxLife;
        MaxBattery = _base.MaxBattery;
        Battery = MaxBattery;
        Power = _base.Power;
        Technique = _base.Technique;
        Defense = _base.Defense;
        Speed = _base.Speed;
        Luck = _base.Luck;
        Memory = _base.Memory;
        Storage = _base.Storage;
        Pocket = _base.Pocket;
        Bag = _base.Bag;
        Money = _base.Money;
        Disk = _base.Disk;
        Key = _base.Key;
        Exp = _base.Exp;

        Level = 1;
        Soul = 0;

        EquipmentList = new List<Equipment>(_base.EquipmentList ?? new List<Equipment>());
        PocketList = new List<Consumable>();
        foreach (Consumable item in _base.PocketList)
        {
            item.Initialize();
            PocketList.Add(item);
        }

        BagItemList = new List<Item>();
        foreach (Consumable item in _base.BagConsumableList)
        {
            item.Initialize();
            BagItemList.Add(item);
        }

        BagItemList.AddRange(_base.BagEquipmentList);
        BagItemList.AddRange(_base.BagTreasureList);

        StorageList = new List<Command>(_base.StorageList ?? new List<Command>());
        TableList = new List<Command>(_base.TableList ?? new List<Command>());

        CoLStatus();
    }

    // TODO : Characterのステータスを更新するメソッド
    public void CoLStatus()
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
}
