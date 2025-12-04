using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewDeliveryQuest", menuName = "Quest/Delivery")]
public class DeliveryQuestBase : QuestBase
{
    public override QuestType QuestType => QuestType.Delivery;

    [Header("Task")] // タスク
    [SerializeField] List<ItemBase> deliveryItemBaseList; //発送・ 納品アイテムリスト

    [Header("Rewards")] // 報酬
    [SerializeField] List<ItemBase> rewardItemBaseList; // 報酬アイテムリスト
    [SerializeField] int coinPrice = 0;
    [SerializeField] int discPrice = 0;

    [Header("Shipping")] // 発送
    [SerializeField, TextArea] string shippingDescription; // 説明
    [SerializeField] PointBase shippingPointBase; // 発送元（ここが入っていればサプライクエストになる）

    public string ShippingDescription { get => shippingDescription; }
    public PointBase ShippingPointBase { get => shippingPointBase; }
    public List<ItemBase> DeliveryItemBaseList { get => deliveryItemBaseList; }
    public List<ItemBase> RewardItemBaseList { get => rewardItemBaseList; }
    public int CoinPrice { get => coinPrice; }
    public int DiscPrice { get => discPrice; }
}
