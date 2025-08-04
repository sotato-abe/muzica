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
        level = 1;
    }

    public virtual Command Clone()
    {
        var copy = new Command(_base);
        copy.level = this.level; // レベルもコピー
        return copy;
    }
}
