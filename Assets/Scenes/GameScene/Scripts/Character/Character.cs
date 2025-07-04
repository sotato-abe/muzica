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

        Level = 1;
        Soul = 0;
        Exp = 0;

        EquipmentList = new List<Equipment>(_base.EquipmentList ?? new List<Equipment>());
        PocketList = new List<Consumable>(_base.PocketList ?? new List<Consumable>());

        BagItemList = new List<Item>();
        BagItemList.AddRange(_base.BagConsumableList);
        BagItemList.AddRange(_base.BagEquipmentList);
        BagItemList.AddRange(_base.BagTreasureList);

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
            DiffMemory += equipment.EquipmentBase.Memory;
            DiffStorage += equipment.EquipmentBase.Storage;
            DiffPocket += equipment.EquipmentBase.Pocket;
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

    public bool AddItem(Item item)
    {
        switch (item)
        {
            case Consumable consumable:
                return TryAddToBag(consumable);
                // if (PocketList.Count < Pocket)
                // {
                //     PocketList.Add(consumable);
                //     return true;
                // }
                // else
                // {
                // return TryAddToBag(consumable);
                // }

            case Equipment equipment:
                return TryAddToBag(equipment);

            case Treasure treasure:
                return TryAddToBag(treasure);

            default:
                Debug.Log("Unknown item type.");
                return false;
        }
    }

    public bool DropItem(Item item)
    {
        if (item == null || !BagItemList.Contains(item))
        {
            Debug.Log("アイテムが見つかりません。");
            return false;
        }

        BagItemList.Remove(item);
        return true;
    }

    private bool TryAddToBag(Item item)
    {
        if (BagItemList.Count < Bag)
        {
            BagItemList.Add(item);
            return true;
        }

        Debug.Log("バッグがいっぱいです。");
        return false;
    }
}
