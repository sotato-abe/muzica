using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeliveryQuest : Quest
{
    // 親クラスの _base をキャストして使う
    public override QuestBase Base => _base;
    public DeliveryQuestBase DeliveryQuestBase => _base as DeliveryQuestBase;

    public DeliveryQuest(DeliveryQuestBase baseData) : base(baseData)
    {
    }

    public override Quest Clone()
    {
        var copy = new DeliveryQuest(DeliveryQuestBase);
        copy.isNew = this.isNew;
        return copy;
    }
}
