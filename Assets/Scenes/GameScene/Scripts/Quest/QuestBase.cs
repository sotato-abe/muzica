using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuestBase : ScriptableObject
{
    [Header("Quest Information")] // クエスト情報
    [SerializeField] new string title; // クエスト名
    [SerializeField] RarityType rarity; // レアリティ
    [SerializeField] FieldType fieldType; // フィールドタイプ
    [SerializeField] PointBase pointBase; // ポイント
    [SerializeField, TextArea] string description; // 説明

    [Header("Conditions")] // 出現条件
    [SerializeField] int validCount; // 有効回数
    [SerializeField] int startYear; // 開始年
    [SerializeField] int startMonth; // 開始月
    [SerializeField] int endYear; // 終了年
    [SerializeField] int endMonth; // 終了月

    public virtual QuestType QuestType => QuestType.Story;

    public string Title { get => title; }
    public RarityType Rarity { get => rarity; }
    public FieldType FieldType { get => fieldType; }
    public PointBase PointBase { get => pointBase; }
    public string Description { get => description; }

    public int ValidCount { get => validCount; }
    public DateTime StartDateTime { get => new DateTime(startYear, startMonth, 1); }
    public DateTime EndDateTime { get => new DateTime(endYear, endMonth, 1); }
}
