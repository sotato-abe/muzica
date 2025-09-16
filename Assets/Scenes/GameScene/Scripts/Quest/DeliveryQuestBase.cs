using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewDeliveryQuest", menuName = "Quest/Delivery")]
public class DeliveryQuestBase : QuestBase
{
    [Header("Task")] // タスク
    [SerializeField] List<Treasure> deliveryItemList; // 運搬アイテムリスト
    [SerializeField, TextArea] string address; // 配達先住所
    [SerializeField] CharacterBase deliveryCharacter; // 配達キャラクター

    public override QuestType questType => QuestType.Delivery;
    public List<Treasure> DeliveryItemList { get => deliveryItemList; }
    public string Address { get => address; }
    public CharacterBase DeliveryCharacter { get => deliveryCharacter; }
}
