using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipmentBase", menuName = "Item/EquipmentBase")]
public class EquipmentBase : ItemBase
{
    [SerializeField] EquipmentType equipmentType; // 装備の種類
    [SerializeField] int attackPower; // 攻撃力（武器やツールの場合）
    [SerializeField] int defensePower; // 防御力（防具の場合）

    [SerializeField] int life;
    [SerializeField] int battery;
    [SerializeField] int power;
    [SerializeField] int technique;
    [SerializeField] int defense;
    [SerializeField] int speed;
    [SerializeField] int luck;
    [SerializeField] int memory;
    [SerializeField] int storage;
    [SerializeField] int pocket;
    [SerializeField] List<EnegyCount> enegyCountList;
    [SerializeField] List<EnegyCost> enegyCostList;

    public EquipmentType EquipmentType { get => equipmentType; }
    public int AttackPower { get => attackPower; }
    public int DefensePower { get => defensePower; }

    public int Life { get => life; }
    public int Battery { get => battery; }
    public int Power { get => power; }
    public int Technique { get => technique; }
    public int Defense { get => defense; }
    public int Speed { get => speed; }
    public int Luck { get => luck; }
    public int Memory { get => memory; }
    public int Storage { get => storage; }
    public int Pocket { get => pocket; }

    public List<EnegyCount> EnegyCountList { get => enegyCountList; }
    public List<EnegyCost> EnegyCostList { get => enegyCostList; }
}
