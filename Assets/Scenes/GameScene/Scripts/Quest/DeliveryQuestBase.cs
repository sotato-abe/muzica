using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewDeliveryQuest", menuName = "Quest/Delivery")]
public class DeliveryQuestBase : QuestBase
{
    [Header("Task")] // タスク
    [SerializeField] List<TreasureBase> deliveryItemBaseList; // 運搬アイテムリスト
    [SerializeField, TextArea] string address; // 配達先住所
    [SerializeField] CharacterBase deliveryCharacter; // 配達キャラクター

    public override QuestType questType => QuestType.Delivery;
    public List<TreasureBase> DeliveryItemBaseList { get => deliveryItemBaseList; }
    public string Address { get => address; }
    public CharacterBase DeliveryCharacter { get => deliveryCharacter; }
}
