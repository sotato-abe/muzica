using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewSupplyQuest", menuName = "Quest/Supply")]
public class SupplyQuestBase : QuestBase
{
    [Header("Task")] // タスク
    [SerializeField] List<Item> supplyItemsList; // 納品アイテムリスト

    public override QuestType questType => QuestType.Supply;
    public List<Item> SupplyItemsList { get => supplyItemsList; }
}
