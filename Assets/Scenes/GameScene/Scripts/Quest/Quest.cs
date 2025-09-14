using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    [SerializeField] protected QuestBase _base;
    public virtual QuestBase Base => _base;
    public bool isNew = true;

    public Quest(QuestBase baseData, bool isNew = true)
    {
        _base = baseData;
        this.isNew = isNew;
    }

    public virtual Quest Clone()
    {
        return new Quest(_base, this.isNew);
    }

    public QuestType GetQuestType()
    {
        return _base.questType;
    }
}
