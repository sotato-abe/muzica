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

    public override ItemType itemType => ItemType.Equipment;
    public List<EnergyCost> EnergyCostList { get => energyCostList; }
    public List<Attack> AttackList { get => attackList; }
    public List<Enchant> EnchantList { get => enchantList; }
}
