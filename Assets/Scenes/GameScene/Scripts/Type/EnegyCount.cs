using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class EnegyCount
{
    public EnegyType type;
    public bool isRecovery = true; // trueならプラス、falseならマイナス
    public int val;
    public int times = 1;

    public EnegyCount(EnegyType type,bool isRecovery, int val, int times = 1)
    {
        this.type = type;
        this.isRecovery = isRecovery;
        this.val = val;
        this.val = times;
    }
}
