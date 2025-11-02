using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewWorkQuest", menuName = "Quest/Work")]
public class WorkQuestBase : QuestBase
{
    [Header("Task")] // タスク
    [SerializeField] int month; // 作業期間（月数）

    [Header("Reward")] // 報酬
    [SerializeField] List<ItemBase> rewardItemBaseList;
    [SerializeField] int coinPrice = 0;
    [SerializeField] int discPrice = 0;

    public override QuestType QuestType => QuestType.Work;

    public int Month { get => month; }
    
    public List<ItemBase> RewardItemBaseList { get => rewardItemBaseList; }
    public int CoinPrice { get => coinPrice; }
    public int DiscPrice { get => discPrice; }
}
