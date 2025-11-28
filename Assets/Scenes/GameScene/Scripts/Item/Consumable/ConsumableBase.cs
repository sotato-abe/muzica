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
}
