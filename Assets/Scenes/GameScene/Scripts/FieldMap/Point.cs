using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Point
{
    [SerializeField] PointBase _base;
    public PointBase Base { get => _base; set => _base = value; }
    public List<Item> ShopItems { get; set; }
    public List<Command> ShopCommands { get; set; }

    public virtual void Init()
    {
        if (_base == null)
        {
            Debug.LogError("Init() failed: _base is null");
            return;
        }

        ShopItems.AddRange(_base.ShopEquipmentList);
        ShopItems.AddRange(_base.ShopConsumableList);
        ShopItems.AddRange(_base.ShopTreasureList);

        ShopCommands.AddRange(_base.ShopCommandList);
    }
}
