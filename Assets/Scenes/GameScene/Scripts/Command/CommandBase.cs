using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCommand", menuName = "Command/CommandBase")]
public class CommandBase : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] RarityType rarity;
    [SerializeField] Sprite sprite;
    [SerializeField, TextArea] string description;
    [SerializeField] int price = 1;
    [SerializeField] List<EnegyCount> enegyAttackList;
    [SerializeField] List<EnegyCost> enegyCostList;
    [SerializeField] List<Enchant> enchantList;

    public string Name { get => name; }
    public RarityType Rarity { get => rarity; }
    public Sprite Sprite { get => sprite; }
    public string Description { get => description; }
    public int Price { get => price; }

    public List<EnegyCount> EnegyAttackList { get => enegyAttackList; }
    public List<EnegyCost> EnegyCostList { get => enegyCostList; }
    public List<Enchant> EnchantList { get => enchantList; }
}
