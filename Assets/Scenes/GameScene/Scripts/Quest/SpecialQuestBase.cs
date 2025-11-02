using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewSpecialQuest", menuName = "Quest/Special")]
public class SpecialQuestBase : QuestBase
{
    public override QuestType QuestType => QuestType.Special;
}
