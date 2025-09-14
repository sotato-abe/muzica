using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuestBase : ScriptableObject
{
    [Header("Quest Information")] // クエスト情報
    [SerializeField] new string name; // クエスト名
    [SerializeField] RarityType rarity; // レアリティ
    [SerializeField, TextArea] string description; // 説明
    [SerializeField, TextArea] string achievementMessage; // 達成メッセージ

    [Header("Conditions")] // 出現条件
    [SerializeField] DateTime startDateTime; // 開始日時
    [SerializeField] DateTime endDateTime; // 終了日時
    [SerializeField] List<Item> optionalItemsList; // 必要アイテムリスト

    [Header("Reward")] // 報酬
    [SerializeField] List<Item> rewardItemsList;
    [SerializeField] int coinPrice = 0;
    [SerializeField] int discPrice = 0;


    public virtual QuestType questType => QuestType.Story;

    public string Name { get => name; }
    public RarityType Rarity { get => rarity; }
    public string Description { get => description; }
    public string AchievementMessage { get => achievementMessage; }

    public DateTime StartDateTime { get => startDateTime; }
    public DateTime EndDateTime { get => endDateTime; }
    public List<Item> OptionalItemsList { get => optionalItemsList; }

    public List<Item> RewardItemsList { get => rewardItemsList; }
    public int CoinPrice { get => coinPrice; }
    public int DiscPrice { get => discPrice; }
}
