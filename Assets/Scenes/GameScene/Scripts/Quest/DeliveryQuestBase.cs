using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewDeliveryQuest", menuName = "Quest/Delivery")]
public class DeliveryQuestBase : QuestBase
{
    [Header("Task")] // タスク
    [SerializeField] List<Item> transportItemsList; // 運搬アイテムリスト
    [SerializeField] Point deliveryPoint; // 配達ポイント

    public override QuestType questType => QuestType.Delivery;
    public List<Item> TransportItemsList { get => transportItemsList; }
    public Point DeliveryPoint { get => deliveryPoint; }
}
