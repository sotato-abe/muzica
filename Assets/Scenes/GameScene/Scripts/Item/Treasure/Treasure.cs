using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Treasure : Item
{
    [SerializeField] TreasureBase _base;
    public override ItemType itemType => ItemType.Treasure;
    public override ItemBase Base => _base;
    public TreasureBase TreasureBase { get => _base; }
}
