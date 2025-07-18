using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class EnergyCount
{
    public EnergyType type;
    public bool isRecovery = true; // trueならプラス、falseならマイナス
    public int val;
    public int times = 1;

    public EnergyCount(EnergyType type, bool isRecovery, int val, int times = 1)
    {
        this.type = type;
        this.isRecovery = isRecovery;
        this.val = val;
        this.val = times;
    }

    public EnergyCount(EnergyCount other)
    {
        type = other.type;
        isRecovery = other.isRecovery;
        val = other.val;
        times = other.times;
    }
}
