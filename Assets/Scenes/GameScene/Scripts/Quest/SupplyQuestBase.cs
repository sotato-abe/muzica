using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewSupplyQuest", menuName = "Quest/Supply")]
public class SupplyQuestBase : QuestBase
{
    [Header("Task")] // タスク
    [SerializeField] List<ItemBase> supplyItemBaseList; // 納品アイテムリスト
    [SerializeField, TextArea] string achievementMessage; // 達成メッセージ

    [Header("Reward")] // 報酬
    [SerializeField] List<ItemBase> rewardItemBaseList;
    [SerializeField] int coinPrice = 0;
    [SerializeField] int discPrice = 0;

    public override QuestType QuestType => QuestType.Supply;
    public List<ItemBase> SupplyItemBaseList { get => supplyItemBaseList; }
    public string AchievementMessage { get => achievementMessage; }

    public List<ItemBase> RewardItemBaseList { get => rewardItemBaseList; }
    public int CoinPrice { get => coinPrice; }
    public int DiscPrice { get => discPrice; }
}
