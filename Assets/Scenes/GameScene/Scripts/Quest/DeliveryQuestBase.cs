using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewDeliveryQuest", menuName = "Quest/Delivery")]
public class DeliveryQuestBase : QuestBase
{
    [Header("Task")] // タスク
    [SerializeField] List<TreasureBase> transportItemBaseList; // 運搬アイテムリスト
    [SerializeField] Point deliveryPoint; // 配達ポイント
    [SerializeField] CharacterBase deliveryCharacter; // 配達キャラクター

    public override QuestType questType => QuestType.Delivery;
    public List<TreasureBase> TransportItemBaseList { get => transportItemBaseList; }
    public Point DeliveryPoint { get => deliveryPoint; }
    public CharacterBase DeliveryCharacter { get => deliveryCharacter; }
}
