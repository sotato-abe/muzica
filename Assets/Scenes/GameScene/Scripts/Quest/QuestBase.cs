using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest/QuestBase")]
public class QuestBase : ScriptableObject
{
    [Header("Quest Information")]
    [SerializeField] new string name;
    [SerializeField] RarityType rarity;
    [SerializeField, TextArea] string description;

    [Header("Reward")]
    [SerializeField] List<Item> itemList;
    [SerializeField] int coinPrice = 1;
    [SerializeField] int discPrice = 0;

    public virtual QuestType questType => QuestType.Delivery;
    public string Name { get => name; }
    public RarityType Rarity { get => rarity; }
    public string Description { get => description; }

    public List<Item> ItemList { get => itemList; }
    public int CoinPrice { get => coinPrice; }
    public int DiscPrice { get => discPrice; }
}
