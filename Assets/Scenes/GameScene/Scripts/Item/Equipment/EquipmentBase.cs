using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipment", menuName = "Item/EquipmentBase")]
public class EquipmentBase : ItemBase
{    
    [Header("Equipment Status")]
    [SerializeField] List<EnergyCost> energyCostList;
    [SerializeField] List<Attack> attackList;
    [SerializeField] List<Enchant> enchantList;

    // 削除予定
    // [SerializeField] EquipmentType equipmentType; // 装備の種類
    // [SerializeField] int life;
    // [SerializeField] int battery;
    // [SerializeField] int power;
    // [SerializeField] int technique;
    // [SerializeField] int defense;
    // [SerializeField] int speed;
    // [SerializeField] int luck;
    // [SerializeField] List<EnergyCount> energyAttackList;
    // [SerializeField] TargetType targetType;

    public override ItemType itemType => ItemType.Equipment;
    // public EquipmentType EquipmentType { get => equipmentType; }
    // public int Life { get => life; }
    // public int Battery { get => battery; }
    // public int Power { get => power; }
    // public int Technique { get => technique; }
    // public int Defense { get => defense; }
    // public int Speed { get => speed; }
    // public int Luck { get => luck; }

    public List<EnergyCost> EnergyCostList { get => energyCostList; }
    public List<Attack> AttackList { get => attackList; }
    public List<Enchant> EnchantList { get => enchantList; }

    // 削除予定
    // public TargetType TargetType { get => targetType; }
    // public List<EnergyCount> EnergyAttackList { get => energyAttackList; }
}
