using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public virtual ItemType itemType => ItemType.Consumable;
    public virtual ItemBase Base => null;
}
