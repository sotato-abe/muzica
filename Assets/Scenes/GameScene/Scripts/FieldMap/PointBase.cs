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
    [SerializeField] List<EquipmentBase> equipmentBaseList;
    [SerializeField] List<ConsumableBase> consumableBaseList;
    [SerializeField] List<TreasureBase> treasureBaseList;
    [SerializeField] List<CommandBase> commandBaseList;

    public string Name { get => name; }
    public Character Owner { get => owner; }
    public string Description { get => description; }
    public Sprite Icon { get => icon; }
    public Sprite Image { get => image; }

    public List<EquipmentBase> ShopEquipmentBaseList { get => equipmentBaseList; }
    public List<ConsumableBase> ShopConsumableBaseList { get => consumableBaseList; }
    public List<TreasureBase> ShopTreasureBaseList { get => treasureBaseList; }
    public List<CommandBase> ShopCommandBaseList { get => commandBaseList; }
}
