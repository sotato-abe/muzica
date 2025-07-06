using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class EnegyCount
{
    public EnegyType type;
    public int val;
    public int times = 1;

    public EnegyCount(EnegyType type, int val, int times = 1)
    {
        this.type = type;
        this.val = val;
        this.val = times;
    }
}
