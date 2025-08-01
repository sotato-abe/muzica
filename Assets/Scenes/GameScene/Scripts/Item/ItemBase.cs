using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] RarityType rarity;
    [SerializeField] Sprite sprite;
    [SerializeField, TextArea] string description;
    [SerializeField] int weight;
    [SerializeField] int coinPrice = 1;
    [SerializeField] int discPrice = 0;

    public string Name { get => name; }
    public RarityType Rarity { get => rarity; }
    public Sprite Sprite { get => sprite; }
    public string Description { get => description; }
    public int Weight { get => weight; }
    public int CoinPrice { get => coinPrice; }
    public int DiscPrice { get => discPrice; }
}
