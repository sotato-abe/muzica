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
    [SerializeField] Sprite image; // ワールドマップで表示されるアイコン
    [SerializeField] List<ItemBase> shopItems = new List<ItemBase>();
    [SerializeField] List<CommandBase> commandItems = new List<CommandBase>();

    public string Name { get => name; }
    public Character Owner { get => owner; }
    public string Description { get => description; }
    public Sprite Icon { get => icon; }
    public Sprite Image { get => image; }
    public List<ItemBase> ShopItems { get => shopItems;}
    public List<CommandBase> ComamndItems { get => commandItems;}
}
