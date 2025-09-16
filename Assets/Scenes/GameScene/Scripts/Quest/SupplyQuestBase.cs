using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewSupplyQuest", menuName = "Quest/Supply")]
public class SupplyQuestBase : QuestBase
{
    [Header("Task")] // タスク
    [SerializeField] List<ItemBase> supplyItemBaseList; // 納品アイテムリスト

    [Header("Reward")] // 報酬
    [SerializeField] List<Item> rewardItemList;
    [SerializeField] int coinPrice = 0;
    [SerializeField] int discPrice = 0;

    public override QuestType questType => QuestType.Supply;
    public List<ItemBase> SupplyItemBaseList { get => supplyItemBaseList; }

    public List<Item> RewardItemList { get => rewardItemList; }
    public int CoinPrice { get => coinPrice; }
    public int DiscPrice { get => discPrice; }
}
