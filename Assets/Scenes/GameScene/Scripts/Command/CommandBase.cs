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
    [SerializeField] List<EnergyCount> energyAttackList;
    [SerializeField] List<EnergyCost> energyCostList;
    [SerializeField] List<Enchant> enchantList;

    public string Name { get => name; }
    public RarityType Rarity { get => rarity; }
    public Sprite Sprite { get => sprite; }
    public string Description { get => description; }
    public int Price { get => price; }

    public List<EnergyCount> EnergyAttackList { get => energyAttackList; }
    public List<EnergyCost> EnergyCostList { get => energyCostList; }
    public List<Enchant> EnchantList { get => enchantList; }
}
