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

    public static Point CreateFrom(PointBase baseData)
    {
        var point = new Point
        {
            Base = baseData,
            ShopItems = new List<Item>(),
            ShopCommands = new List<Command>()
        };

        point.ShopItems.AddRange(baseData.ShopEquipmentList);

        foreach (var consumableBase in baseData.ShopConsumableBaseList)
        {
            var consumable = new Consumable(consumableBase);
            consumable.Initialize();
            point.ShopItems.Add(consumable);
        }

        point.ShopItems.AddRange(baseData.ShopTreasureList);
        point.ShopCommands.AddRange(baseData.ShopCommandList);

        return point;
    }
}
