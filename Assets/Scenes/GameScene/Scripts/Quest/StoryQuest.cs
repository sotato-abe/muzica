using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoryQuest : Quest
{
    // 親クラスの _base をキャストして使う
    public override QuestBase Base => _base;
    public StoryQuestBase StoryQuestBase => _base as StoryQuestBase;

    public StoryQuest(StoryQuestBase baseData) : base(baseData)
    {
    }

    public override Quest Clone()
    {
        var copy = new StoryQuest(StoryQuestBase);
        return copy;
    }
}
