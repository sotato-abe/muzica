using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuestDatabase : MonoBehaviour
{
    public static QuestDatabase Instance { get; private set; }
    [SerializeField] List<StoryQuestBase> storyQuestDataList;
    [SerializeField] List<SupplyQuestBase> supplyQuestDataList;
    [SerializeField] List<DeliveryQuestBase> deliveryQuestDataList;
    [SerializeField] List<ExterminationQuestBase> exterminationQuestDataList;
    [SerializeField] List<SpecialQuestBase> specialQuestDataList;
    List<QuestBase> QuestDataList;

    private List<int> finishedQuestIds = new List<int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン切り替えても残す
        }
        else
        {
            Destroy(gameObject); // 重複防止
        }

        QuestDataList = new List<QuestBase>();
        QuestDataList.AddRange(storyQuestDataList);
        QuestDataList.AddRange(supplyQuestDataList);
        QuestDataList.AddRange(deliveryQuestDataList);
        QuestDataList.AddRange(exterminationQuestDataList);
        QuestDataList.AddRange(specialQuestDataList);
    }

    public Quest GetQuestById(int QuestId)
    {
        if (QuestId < 0 || QuestId >= QuestDataList.Count)
        {
            Debug.LogWarning("Invalid Quest ID: " + QuestId);
            return null;
        }

        QuestBase baseData = QuestDataList[QuestId];
        switch (baseData.QuestType)
        {
            case QuestType.Story:
                return new StoryQuest((StoryQuestBase)baseData);
            case QuestType.Supply:
                return new SupplyQuest((SupplyQuestBase)baseData);
            case QuestType.Delivery:
                return new DeliveryQuest((DeliveryQuestBase)baseData);
            case QuestType.Extermination:
                return new ExterminationQuest((ExterminationQuestBase)baseData);
            case QuestType.Special:
                return new SpecialQuest((SpecialQuestBase)baseData);
            default:
                Debug.LogError("Unknown Quest type: " + baseData.QuestType);
                return null;
        }
    }

    public List<Quest> GetActiveQuestsByTime(DateTime targetTime)
    {
        List<Quest> activeQuests = new List<Quest>();
        for (int i = 0; i < QuestDataList.Count; i++)
        {
            if (finishedQuestIds.Contains(i))
            {
                continue; // クリア済みクエストはスキップ
            }
            QuestBase questBase = QuestDataList[i];
            if (questBase.StartDateTime == null || targetTime >= questBase.StartDateTime)
            {
                if (questBase.EndDateTime == null || targetTime <= questBase.EndDateTime)
                {
                    Quest quest = GetQuestById(i);
                    if (quest != null)
                    {
                        activeQuests.Add(quest);
                    }
                }
            }
        }
        return activeQuests;
    }

    public List<Quest> GetActiveQuestByField(DateTime targetTime, FieldType fieldType)
    {
        List<Quest> activeQuests = new List<Quest>();
        for (int i = 0; i < QuestDataList.Count; i++)
        {
            if (finishedQuestIds.Contains(i))
            {
                continue; // クリア済みクエストはスキップ
            }
            QuestBase questBase = QuestDataList[i];
            if (questBase.FieldType != fieldType)
            {
                continue; // フィールドタイプが一致しない場合はスキップ
            }
            if (questBase.StartDateTime == null || targetTime >= questBase.StartDateTime)
            {
                if (questBase.EndDateTime == null || targetTime <= questBase.EndDateTime)
                {
                    Quest quest = GetQuestById(i);
                    if (quest != null)
                    {
                        activeQuests.Add(quest);
                    }
                }
            }
        }
        return activeQuests;
    }

    public List<Quest> GetActiveQuestsByPoint(DateTime targetTime, PointBase pointBase)
    {
        List<Quest> activeQuests = new List<Quest>();
        for (int i = 0; i < QuestDataList.Count; i++)
        {
            if (finishedQuestIds.Contains(i))
            {
                continue; // クリア済みクエストはスキップ
            }
            QuestBase questBase = QuestDataList[i];
            if (questBase.PointBase != pointBase)
            {
                continue; // ポイントが一致しない場合はスキップ
            }
            if (questBase.StartDateTime == null || targetTime >= questBase.StartDateTime)
            {
                if (questBase.EndDateTime == null || targetTime <= questBase.EndDateTime)
                {
                    Quest quest = GetQuestById(i);
                    if (quest != null)
                    {
                        activeQuests.Add(quest);
                    }
                }
            }
        }
        return activeQuests;
    }

    public int GetQuestId(QuestBase Quest)
    {
        if (Quest == null)
        {
            return -1;
        }
        return QuestDataList.IndexOf(Quest);
    }

    public void MarkQuestAsFinished(int questId)
    {
        if (!finishedQuestIds.Contains(questId))
        {
            finishedQuestIds.Add(questId);
        }
    }
}