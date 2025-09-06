using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    [SerializeField] protected ItemBase _base;
    public virtual ItemBase Base => _base;
    public bool isNew = true;

    public Item(ItemBase baseData, bool isNew = true)
    {
        _base = baseData;
        this.isNew = isNew;
    }

    public virtual Item Clone()
    {
        return new Item(_base, this.isNew);
    }

    public ItemType GetItemType()
    {
        return _base.itemType;
    }
}
