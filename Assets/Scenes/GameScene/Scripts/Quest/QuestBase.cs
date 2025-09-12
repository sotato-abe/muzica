using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest/QuestBase")]
public class QuestBase : ScriptableObject
{
    [Header("Quest Information")] // クエスト情報
    [SerializeField] new string name;
    [SerializeField] RarityType rarity;
    [SerializeField] QuestType questType;
    [SerializeField, TextArea] string description;
    [SerializeField] List<Item> orderedItemsList;

    [Header("Conditions")] // 出現条件
    [SerializeField] DateTime startDateTime;
    [SerializeField] DateTime endDateTime;
    [SerializeField] List<Item> optionalItemsList;

    [Header("Reward")] // 報酬
    [SerializeField] List<Item> rewardItemsList;
    [SerializeField] int coinPrice = 1;
    [SerializeField] int discPrice = 0;


    public string Name { get => name; }
    public RarityType Rarity { get => rarity; }
    public QuestType QuestType => questType;
    public string Description { get => description; }
    public List<Item> OrderedItemsList { get => orderedItemsList; }

    public DateTime StartDateTime { get => startDateTime; }
    public DateTime EndDateTime { get => endDateTime; }
    public List<Item> OptionalItemsList { get => optionalItemsList; }

    public List<Item> RewardItemsList { get => rewardItemsList; }
    public int CoinPrice { get => coinPrice; }
    public int DiscPrice { get => discPrice; }
}
