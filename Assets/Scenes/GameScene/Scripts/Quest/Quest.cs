using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    [SerializeField] protected QuestBase _base;
    public virtual QuestBase Base => _base;
    public bool isCompleted = true;

    public Quest(QuestBase baseData)
    {
        _base = baseData;
        this.isCompleted = false;
    }

    public virtual Quest Clone()
    {
        return new Quest(_base);
    }

    public QuestType GetQuestType()
    {
        return _base.questType;
    }

    public bool IsCompleted()
    {
        return isCompleted;
    }
}
