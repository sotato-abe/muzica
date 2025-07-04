using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Consumable : Item
{
    [SerializeField] ConsumableBase _base;

    public override ItemType itemType => ItemType.Consumable;
    public override ItemBase Base => _base;
    public ConsumableBase ConsumableBase { get => _base; }

    public Consumable(ConsumableBase baseData)
    {
        _base = baseData;
    }
}
