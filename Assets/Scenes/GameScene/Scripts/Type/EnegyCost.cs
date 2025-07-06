using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class EnegyCost
{
    public EnegyType type;
    public int val;

    public EnegyCost(EnegyType type, int val)
    {
        this.type = type;
        this.val = val;
    }
}
