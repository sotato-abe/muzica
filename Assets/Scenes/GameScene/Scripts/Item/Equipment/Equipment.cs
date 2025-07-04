using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Equipment : Item
{
    [SerializeField] EquipmentBase _base;
    [SerializeField] int level = 1;

    public override ItemType itemType => ItemType.Equipment;
    public override ItemBase Base => _base; // EquipmentBase は ItemBase を継承している前提
    public EquipmentBase EquipmentBase => _base;
    public int Level => level;

    public Equipment(EquipmentBase baseData)
    {
        _base = baseData;
    }
}
