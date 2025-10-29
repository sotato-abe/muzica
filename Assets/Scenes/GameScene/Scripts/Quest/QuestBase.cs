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
    [SerializeField] bool isOneTimeOnly; //TODO 一度きりフラグ　ストーリークエストなどのフラグに使う
    [SerializeField] DateTime startDateTime; // 開始日時
    [SerializeField] DateTime endDateTime; // 終了日時
    [SerializeField] List<Item> optionalItemsList; // 必要アイテムリスト

    public virtual QuestType QuestType => QuestType.Story;

    public string Title { get => title; }
    public RarityType Rarity { get => rarity; }
    public FieldType FieldType { get => fieldType; }
    public PointBase PointBase { get => pointBase; }
    public string Description { get => description; }

    public DateTime StartDateTime { get => startDateTime; }
    public DateTime EndDateTime { get => endDateTime; }
    public List<Item> OptionalItemsList { get => optionalItemsList; }
}
