using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Treasure")]
public class TreasureBase : ItemBase
{
    public override ItemType itemType => ItemType.Treasure;
}
