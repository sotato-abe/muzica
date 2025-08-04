using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    [SerializeField] protected ItemBase _base;
    public virtual ItemBase Base => _base;

    public Item(ItemBase baseData)
    {
        _base = baseData;
    }

    public virtual Item Clone()
    {
        return new Item(_base);
    }
}
