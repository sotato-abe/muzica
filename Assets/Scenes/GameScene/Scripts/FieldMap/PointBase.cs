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
    [SerializeField] List<ConsumableBase> consumableBaseList;
    [SerializeField] List<Treasure> treasureList;
    [SerializeField] List<Command> commandList;

    public string Name { get => name; }
    public Character Owner { get => owner; }
    public string Description { get => description; }
    public Sprite Icon { get => icon; }
    public Sprite Image { get => image; }

    public List<Equipment> ShopEquipmentList { get => equipmentList; }
    public List<ConsumableBase> ShopConsumableBaseList { get => consumableBaseList; }
    public List<Treasure> ShopTreasureList { get => treasureList; }
    public List<Command> ShopCommandList { get => commandList; }
}
