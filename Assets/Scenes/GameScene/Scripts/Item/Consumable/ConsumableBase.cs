using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Consumable")]
public class ConsumableBase : ItemBase
{
    [SerializeField] int usableCount = 1; // 使用回数
    [SerializeField] Attack attack;
    [SerializeField] List<Enchant> enchantList;
    

    public override ItemType itemType => ItemType.Consumable;
    public int UsableCount { get => usableCount; }
    public Attack Attack { get => attack; }   
    public List<Enchant> EnchantList { get => enchantList; }

    // 削除予定
    [SerializeField] TargetType targetType;
    [SerializeField] ConsumableType consumableType;
    [SerializeField] List<EnergyCount> energyAttackList;
    public TargetType TargetType { get => targetType; }
    public ConsumableType ConsumableType { get => consumableType; }
    public List<EnergyCount> EnergyAttackList { get => energyAttackList; }
}
