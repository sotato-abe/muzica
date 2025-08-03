using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Treasure : Item
{
    public override ItemBase Base => _base;
    public TreasureBase TreasureBase => _base as TreasureBase;
    public Treasure(TreasureBase baseData) : base(baseData)
    {
        // 必要なら初期化ロジックもここに
    }

    public override Item Clone()
    {
        var copy = new Treasure(TreasureBase);
        // 必要なら他のプロパティもコピー
        return copy;
    }
}
