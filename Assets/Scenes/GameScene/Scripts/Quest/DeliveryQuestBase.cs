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

    [Header("Reward")] // 報酬
    [SerializeField] List<Item> rewardItemList;
    [SerializeField] int coinPrice = 0;
    [SerializeField] int discPrice = 0;

    public override QuestType questType => QuestType.Delivery;
    public List<TreasureBase> DeliveryItemBaseList { get => deliveryItemBaseList; }
    public string Address { get => address; }
    public CharacterBase DeliveryCharacter { get => deliveryCharacter; }

    public List<Item> RewardItemList { get => rewardItemList; }
    public int CoinPrice { get => coinPrice; }
    public int DiscPrice { get => discPrice; }
}
