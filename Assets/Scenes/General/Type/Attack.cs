using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack
{
    [SerializeField] private AttackType attackType;
    [SerializeField] private int val;
    [SerializeField] private float times = 1f;

    public AttackType AttackType { get => attackType; }
    public int Val { get => val; }
    public float Times { get => times; }

    // コピーコンストラクタを追加
    public Attack(Attack other)
    {
        this.attackType = other.attackType;
        this.val = other.val;
        this.times = other.times;
    }

    public void AttackUpdate(Attack attack)
    {
        if (attackType != attack.AttackType) return;
        val += attack.Val;
        times += attack.Times;
    }
}
