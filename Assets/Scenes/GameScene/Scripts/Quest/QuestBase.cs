using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuestBase : ScriptableObject
{
    [Header("Quest Information")] // クエスト情報
    [SerializeField] new string title; // クエスト名
    [SerializeField] RarityType rarity; // レアリティ
    [SerializeField, TextArea] string description; // 説明
    [SerializeField, TextArea] string achievementMessage; // 達成メッセージ

    [Header("Conditions")] // 出現条件
    [SerializeField] DateTime startDateTime; // 開始日時
    [SerializeField] DateTime endDateTime; // 終了日時
    [SerializeField] List<Item> optionalItemsList; // 必要アイテムリスト

    public virtual QuestType questType => QuestType.Story;

    public string Title { get => title; }
    public RarityType Rarity { get => rarity; }
    public string Description { get => description; }
    public string AchievementMessage { get => achievementMessage; }

    public DateTime StartDateTime { get => startDateTime; }
    public DateTime EndDateTime { get => endDateTime; }
    public List<Item> OptionalItemsList { get => optionalItemsList; }
}
