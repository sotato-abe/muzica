using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Command
{
    [SerializeField] CommandBase _base;
    [SerializeField] int level = 1;
    public bool isNew = true;

    public CommandBase Base => _base; // CommandBase は ScriptableObject を継承している前提
    public int Level => level;

    public Command(CommandBase baseData, bool isNew = true)
    {
        _base = baseData;
        level = 1;
        this.isNew = isNew;
    }

    public virtual Command Clone()
    {
        var copy = new Command(_base);
        copy.level = this.level; // レベルもコピー
        copy.isNew = this.isNew;
        return copy;
    }

    public TotalAttack GetTotalAttack()
    {
        TotalAttack totalAttack = new TotalAttack();
        totalAttack.AttackList.AddRange(Base.AttackList);
        totalAttack.EnchantList.AddRange(Base.EnchantList);
        return totalAttack;
    }
}
