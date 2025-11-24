using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class TotalAttack
{
    public List<Attack> AttackList { get; set; } = new List<Attack>();
    public List<Enchant> EnchantList { get; set; } = new List<Enchant>();
}

public static class TotalAttackExtensions
{
    public static TotalAttack GetPositiveTotalAttack(this TotalAttack totalAttack, bool isPositive)
    {
        TotalAttack positiveTotalAttack = new TotalAttack();
        positiveTotalAttack.AttackList = totalAttack.GetPositiveAttack(isPositive);
        positiveTotalAttack.EnchantList = totalAttack.GetPositiveEnchant(isPositive);
        return positiveTotalAttack;
    }

    public static List<Attack> GetPositiveAttack(this TotalAttack totalAttack,bool isPositive)
    {
        List<Attack> attacks = new List<Attack>();
        foreach (var attack in totalAttack.AttackList)
        {
            if (attack.AttackType.AssigneeSelf() == isPositive)
            {
                attacks.Add(attack);
            }
        }
        return attacks;
    }

    public static List<Enchant> GetPositiveEnchant(this TotalAttack totalAttack, bool isPositive)
    {
        List<Enchant> enchants = new List<Enchant>();
        foreach (var enchant in totalAttack.EnchantList)
        {
            if (enchant.Type.AssigneeSelf() == isPositive)
            {
                enchants.Add(enchant);
            }
        }
        return enchants;
    }

    public static bool HasPositiveAttack(this TotalAttack totalAttack, bool isPositive)
    {
        foreach (var attack in totalAttack.AttackList)
        {
            if (attack.AttackType.AssigneeSelf() == isPositive)
            {
                return true;
            }
        }
        return false;
    }

    public static bool HasPositiveEnchant(this TotalAttack totalAttack, bool isPositive)
    {
        foreach (var enchant in totalAttack.EnchantList)
        {
            if (enchant.Type.AssigneeSelf() == isPositive)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsGroupAttack(this TotalAttack totalAttack)
    {
        foreach (var attack in totalAttack.AttackList)
        {
            if (attack.AttackType.IsGroupAttack())
            {
                return true;
            }
        }
        return false;
    }
}

