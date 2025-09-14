using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExterminationQuest : Quest
{
    // 親クラスの _base をキャストして使う
    public override QuestBase Base => _base;
    public ExterminationQuestBase ExterminationQuestBase => _base as ExterminationQuestBase;

    public ExterminationQuest(ExterminationQuestBase baseData) : base(baseData)
    {
    }

    public override Quest Clone()
    {
        var copy = new ExterminationQuest(ExterminationQuestBase);
        copy.isNew = this.isNew;
        return copy;
    }
}
