using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewPoint", menuName = "Field/PointBase")]
public class PointBase : ScriptableObject
{
    [Header("Shop information")]
    [SerializeField] string pointName;
    [SerializeField] CharacterBase owner;
    [SerializeField, TextArea] string description;
    [SerializeField] Sprite icon; // ワールドマップで表示されるアイコン
    [SerializeField] Sprite image;

    [Header("Merchandise")]
    [SerializeField] List<ItemBase> shopItemBaseList;
    [SerializeField] List<CommandBase> commandBaseList;

    public string Name { get => pointName; }
    public CharacterBase Owner { get => owner; }
    public string Description { get => description; }
    public Sprite Icon { get => icon; }
    public Sprite Image { get => image; }

    public List<ItemBase> ShopItemBaseList { get => shopItemBaseList; }
    public List<CommandBase> ShopCommandBaseList { get => commandBaseList; }
}
