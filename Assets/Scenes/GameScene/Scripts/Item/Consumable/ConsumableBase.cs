using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Consumable")]
public class ConsumableBase : ItemBase
{
    [SerializeField] TargetType targetType;
    [SerializeField] ConsumableType consumableType;
    [SerializeField] int usableCount = 1; // 使用回数
    [SerializeField] List<Enchant> enchantList;
    [SerializeField] List<EnergyCount> energyAttackList;

    public TargetType TargetType { get => targetType; }
    public ConsumableType ConsumableType { get => consumableType; }
    public int UsableCount { get => usableCount; }
    public List<Enchant> EnchantList { get => enchantList; }
    public List<EnergyCount> EnergyAttackList { get => energyAttackList; }
}
