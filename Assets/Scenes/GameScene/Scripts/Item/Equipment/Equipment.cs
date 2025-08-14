using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Equipment : Item
{
    [SerializeField] int level = 1;

    // 親クラスの _base をキャストして使う
    public override ItemBase Base => _base;
    public EquipmentBase EquipmentBase => _base as EquipmentBase;
    public int Level => level;

    public Equipment(EquipmentBase baseData) : base(baseData)
    {
        level = 1; // 初期レベルを設定
    }

    public override Item Clone()
    {
        var copy = new Equipment(EquipmentBase);
        copy.level = this.level; // レベルもコピー
        copy.isNew = this.isNew;
        return copy;
    }
}
