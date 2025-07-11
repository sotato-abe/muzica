using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class EnergyCost
{
    public EnergyType type;
    public int val;

    public EnergyCost(EnergyType type, int val)
    {
        this.type = type;
        this.val = val;
    }
}
