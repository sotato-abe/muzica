using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewPoint", menuName = "Field/PointBase")]
public class PointBase : ScriptableObject
{
    [SerializeField] string name;
    [SerializeField] Character owner;
    [SerializeField, TextArea] string description;
    [SerializeField] Sprite icon; // ワールドマップで表示されるアイコン
    [SerializeField] Sprite image;
    [SerializeField] List<Equipment> equipmentList;
    [SerializeField] List<Consumable> consumableList;
    [SerializeField] List<Treasure> treasureList;
    [SerializeField] List<Command> commandList;

    public string Name { get => name; }
    public Character Owner { get => owner; }
    public string Description { get => description; }
    public Sprite Icon { get => icon; }
    public Sprite Image { get => image; }

    public List<Equipment> ShopEquipmentList { get => equipmentList; }
    public List<Consumable> ShopConsumableList { get => consumableList; }
    public List<Treasure> ShopTreasureList { get => treasureList; }
    public List<Command> ShopCommandList { get => commandList; }

    public Point ToPoint()
    {
        Point point = new Point();
        point.Base = this; // PointBaseをPointに設定
        point.ShopItems = new List<Item>();
        point.ShopCommands = new List<Command>();

        // アイテムとコマンドをショップリストに追加
        point.ShopItems.AddRange(equipmentList);
        point.ShopItems.AddRange(consumableList);
        point.ShopItems.AddRange(treasureList);
        point.ShopCommands.AddRange(commandList);

        return point;
    }
}
