using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Consumable")]
public class ConsumableBase : ItemBase
{
    [SerializeField] TargetType targetType;
    [SerializeField] ConsumableType consumableType;
    [SerializeField] List<Enchant> enchantList;
    [SerializeField] List<EnegyCount> enegyAttackList;

    public TargetType TargetType { get => targetType; }
    public ConsumableType ConsumableType { get => consumableType; }
    public List<Enchant> EnchantList { get => enchantList; }
    public List<EnegyCount> EnegyAttackList { get => enegyAttackList; }
}
