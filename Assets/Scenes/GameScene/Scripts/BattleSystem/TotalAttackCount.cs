using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class TotalAttackCount
{
    public TargetType TargetType { get; set; } = TargetType.Individual;
    public List<EnergyCount> EnergyAttackList { get; set; } = new List<EnergyCount>();
    public List<Enchant> EnchantList { get; set; } = new List<Enchant>();
}

