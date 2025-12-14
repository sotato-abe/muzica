using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewInformation", menuName = "Information")]
public class InformationBase : ScriptableObject
{
    [Header("Information Information")] // 情報
    [SerializeField] string title; // タイトル
    [SerializeField] Sprite sprite;
    [SerializeField] FieldType fieldType; // フィールドタイプ
    [SerializeField] FieldBase fieldBase; // フィールド
    [SerializeField] PointBase pointBase; // ポイント
    [SerializeField, TextArea] string description; // 説明

    [Header("Conditions")] // 出現条件
    [SerializeField] int startYear; // 開始年
    [SerializeField] int startMonth; // 開始月

    public virtual InformationType InformationType => InformationType.Story;

    public string Title { get => title; }
    public Sprite Sprite { get => sprite; }
    public FieldType FieldType { get => fieldType; }
    public FieldBase FieldBase { get => fieldBase; }
    public PointBase PointBase { get => pointBase; }
    public string Description { get => description; }

    public DateTime StartDateTime { get => new DateTime(startYear, startMonth, 1); }
}
