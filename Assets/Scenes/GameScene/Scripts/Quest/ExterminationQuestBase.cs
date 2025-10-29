using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewExterminationQuest", menuName = "Quest/Extermination")]
public class ExterminationQuestBase : QuestBase
{
    [Header("Task")] // タスク
    [SerializeField] List<CharacterBase> exterminationCharactersList; // 討伐キャラクターリスト

    public override QuestType QuestType => QuestType.Extermination;
    public List<CharacterBase> ExterminationCharactersList { get => exterminationCharactersList; }
}
