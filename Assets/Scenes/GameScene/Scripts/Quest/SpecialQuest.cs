using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpecialQuest : Quest
{
    // 親クラスの _base をキャストして使う
    public override QuestBase Base => _base;
    public SpecialQuestBase SpecialQuestBase => _base as SpecialQuestBase;

    public SpecialQuest(SpecialQuestBase baseData) : base(baseData)
    {
    }

    public override Quest Clone()
    {
        var copy = new SpecialQuest(SpecialQuestBase);
        return copy;
    }
}
