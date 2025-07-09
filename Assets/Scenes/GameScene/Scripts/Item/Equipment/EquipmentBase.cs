using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipment", menuName = "Item/EquipmentBase")]
public class EquipmentBase : ItemBase
{
    [SerializeField] EquipmentType equipmentType; // 装備の種類
    [SerializeField] int life;
    [SerializeField] int battery;
    [SerializeField] int power;
    [SerializeField] int technique;
    [SerializeField] int defense;
    [SerializeField] int speed;
    [SerializeField] int luck;

    [SerializeField] TargetType targetType;
    [SerializeField] List<EnegyCost> enegyCostList;
    [SerializeField] List<Enchant> enchantList;
    [SerializeField] List<EnegyCount> enegyAttackList;

    public EquipmentType EquipmentType { get => equipmentType; }
    public int Life { get => life; }
    public int Battery { get => battery; }
    public int Power { get => power; }
    public int Technique { get => technique; }
    public int Defense { get => defense; }
    public int Speed { get => speed; }
    public int Luck { get => luck; }

    public TargetType TargetType { get => targetType; }
    public List<EnegyCost> EnegyCostList { get => enegyCostList; }
    public List<Enchant> EnchantList { get => enchantList; }
    public List<EnegyCount> EnegyAttackList { get => enegyAttackList; }
}
