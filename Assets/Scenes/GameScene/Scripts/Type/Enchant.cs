using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Enchant
{
    [SerializeField] EnchantType type;
    [SerializeField] int val;

    public EnchantType Type { get => type; set => type = value; }
    public int Val { get => val; set => val = value; }

    public Enchant(EnchantType type, int val)
    {
        this.type = type;
        this.val = val;
    }

    public Enchant(Enchant other)
    {
        Type = other.Type;
        Val = other.Val;
    }

    public void EnchantUpdate(Enchant enchant)
    {
        if (type != enchant.Type) return;
        val += enchant.Val;
    }

    public bool Reuduce1Enchant()
    {
        return ReduceEnchant(1);
    }

    public bool ReduceEnchant(int amount)
    {
        Val -= amount;
        if (Val <= 0)
        {
            Val = 0;
            return false;
        }
        return true;
    }
}
