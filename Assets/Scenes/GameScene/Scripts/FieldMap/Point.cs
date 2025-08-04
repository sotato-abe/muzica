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

        foreach (var equipmentBase in baseData.ShopEquipmentBaseList)
        {
            var equipment = new Equipment(equipmentBase);
            point.ShopItems.Add(equipment);
        }

        foreach (var consumableBase in baseData.ShopConsumableBaseList)
        {
            var consumable = new Consumable(consumableBase);
            point.ShopItems.Add(consumable);
        }

        foreach (var treasureBase in baseData.ShopTreasureBaseList)
        {
            var treasure = new Treasure(treasureBase);
            point.ShopItems.Add(treasure);
        }

        foreach (var commandBase in baseData.ShopCommandBaseList)
        {
            var command = new Command(commandBase);
            point.ShopCommands.Add(command);
        }

        return point;
    }
}
