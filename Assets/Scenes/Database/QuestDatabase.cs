using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

public class QuestDatabase : MonoBehaviour
{
    public static QuestDatabase Instance { get; private set; }
    
    [SerializeField] List<StoryQuestBase> storyQuestDataList;
    [SerializeField] List<SupplyQuestBase> supplyQuestDataList;
    [SerializeField] List<DeliveryQuestBase> deliveryQuestDataList;
    [SerializeField] List<ExterminationQuestBase> exterminationQuestDataList;
    [SerializeField] List<WorkQuestBase> workQuestDataList;
    [SerializeField] List<SpecialQuestBase> specialQuestDataList;
    List<QuestBase> QuestDataList;

    private List<Vector2Int> ExecutionQuests = new List<Vector2Int>(); //実行済みのクエストID・実行回数

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
        QuestDataList.AddRange(workQuestDataList);
        QuestDataList.AddRange(specialQuestDataList);
    }

    public Quest GetQuestById(int QuestId)
    {
        if (QuestId < 0 || QuestId >= QuestDataList.Count)
        {
            UnityEngine.Debug.LogWarning("Invalid Quest ID: " + QuestId);
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
            case QuestType.Work:
                return new WorkQuest((WorkQuestBase)baseData);
            case QuestType.Special:
                return new SpecialQuest((SpecialQuestBase)baseData);
            default:
                UnityEngine.Debug.LogError("Unknown Quest type: " + baseData.QuestType);
                return null;
        }
    }

    public List<Quest> GetActiveQuestsByTime(DateTime targetTime)
    {
        List<Quest> activeQuests = new List<Quest>();
        List<Item> playerItems = PlayerController.Instance.GetItemList();

        for (int i = 0; i < QuestDataList.Count; i++)
        {
            QuestBase questBase = QuestDataList[i];
            if (ExecutionQuests.Contains(new Vector2Int(i, questBase.ValidCount)) && questBase.ValidCount >= questBase.ValidCount)
            {
                continue; // 実行済みクエストはスキップ
            }
            if (IsActiveDateTime(questBase, targetTime))
            {
                if (IsActiveItem(questBase, playerItems))
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
        List<Item> playerItems = PlayerController.Instance.GetItemList();

        for (int i = 0; i < QuestDataList.Count; i++)
        {
            QuestBase questBase = QuestDataList[i];
            if (ExecutionQuests.Contains(new Vector2Int(i, questBase.ValidCount)) && questBase.ValidCount >= questBase.ValidCount)
            {
                continue; // クリア済みクエストはスキップ
            }
            if (questBase.FieldType != fieldType)
            {
                continue; // フィールドタイプが一致しない場合はスキップ
            }
            if (IsActiveDateTime(questBase, targetTime))
            {
                if (IsActiveItem(questBase, playerItems))
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
        List<Item> playerItems = PlayerController.Instance.GetItemList();

        for (int i = 0; i < QuestDataList.Count; i++)
        {
            QuestBase questBase = QuestDataList[i];
            if (ExecutionQuests.Contains(new Vector2Int(i, questBase.ValidCount)) && questBase.ValidCount >= questBase.ValidCount)
            {
                continue; // 実行済みクエストはスキップ
            }
            if (questBase.PointBase != pointBase)
            {
                continue; // ポイントが一致しない場合はスキップ
            }
            if (IsActiveDateTime(questBase, targetTime))
            {
                if (IsActiveItem(questBase, playerItems))
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

    private bool IsActiveDateTime(QuestBase questBase, DateTime targetTime)
    {
        if (questBase.StartDateTime == null || targetTime >= questBase.StartDateTime)
        {
            if (questBase.EndDateTime == null || targetTime <= questBase.EndDateTime)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsActiveItem(QuestBase questBase, List<Item> playerItems)
    {
        foreach (Item requiredItem in questBase.OptionalItemsList)
        {
            if (!playerItems.Contains(requiredItem))
            {
                return false; // 必要アイテムが不足している場合
            }
        }
        return true;
    }

    public int GetQuestId(QuestBase Quest)
    {
        if (Quest == null)
        {
            return -1;
        }
        return QuestDataList.IndexOf(Quest);
    }

    public void MarkQuestAsFinished(QuestBase questBase)
    {
        int questId = GetQuestId(questBase);
        if (questId == -1)
        {
            UnityEngine.Debug.LogWarning("Attempted to mark an invalid quest as finished.");
            return;
        }
        MarkQuestAsFinishedById(questId);
    }

    public void MarkQuestAsFinishedById(int questId)
    {
        if (ExecutionQuests.Contains(new Vector2Int(questId, 0)))
        {
            int index = ExecutionQuests.IndexOf(new Vector2Int(questId, 0));
            ExecutionQuests[index] = new Vector2Int(questId, ExecutionQuests[index].y + 1);
        }
        else
        {
            ExecutionQuests.Add(new Vector2Int(questId, 1));
        }
    }
}