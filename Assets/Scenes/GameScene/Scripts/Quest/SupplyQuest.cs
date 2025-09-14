using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SupplyQuest : Quest
{
    // 親クラスの _base をキャストして使う
    public override QuestBase Base => _base;
    public SupplyQuestBase SupplyQuestBase => _base as SupplyQuestBase;

    public SupplyQuest(SupplyQuestBase baseData) : base(baseData)
    {
    }

    public override Quest Clone()
    {
        var copy = new SupplyQuest(SupplyQuestBase);
        return copy;
    }
}
