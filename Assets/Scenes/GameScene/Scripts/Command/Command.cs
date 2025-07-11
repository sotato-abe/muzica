using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Command
{
    [SerializeField] CommandBase _base;
    [SerializeField] int level = 1;

    public CommandBase Base => _base; // CommandBase は ScriptableObject を継承している前提
    public int Level => level;

    public Command(CommandBase baseData)
    {
        _base = baseData;
    }

    public string Name => _base.Name;
    public RarityType Rarity => _base.Rarity;
    public Sprite Sprite => _base.Sprite;
    public string Description => _base.Description;
    public int Price => _base.Price;

    public List<EnergyCount> EnergyAttackList => _base.EnergyAttackList;
    public List<EnergyCost> EnergyCostList => _base.EnergyCostList;
    public List<Enchant> EnchantList => _base.EnchantList;
}
