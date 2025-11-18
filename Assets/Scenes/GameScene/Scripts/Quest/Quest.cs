using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    [SerializeField] protected QuestBase _base;
    public virtual QuestBase Base => _base;
    public bool isCompleted = true;
    public QuestStatus Status { get; set; }

    public Quest(QuestBase baseData)
    {
        _base = baseData;
        Status = QuestStatus.Available;
        this.isCompleted = false;
    }

    public virtual void Init()
    {
        Status = QuestStatus.Available;
        // 初期化処理をここに追加
    }

    public virtual Quest Clone()
    {
        return new Quest(_base);
    }

    public QuestType GetQuestType()
    {
        return _base.QuestType;
    }

    public bool IsCompleted()
    {
        return isCompleted;
    }
}
