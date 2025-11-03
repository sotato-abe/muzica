using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

public class InformationDatabase : MonoBehaviour
{
    public static InformationDatabase Instance { get; private set; }

    [SerializeField] List<InformationBase> InformationDataList;

    private List<Vector2Int> ExecutionInformations = new List<Vector2Int>(); //実行済みのクエストID・実行回数

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
    }

    public Information GetInformationById(int InformationId)
    {
        if (InformationId < 0 || InformationId >= InformationDataList.Count)
        {
            UnityEngine.Debug.LogWarning("Invalid Information ID: " + InformationId);
            return null;
        }

        InformationBase baseData = InformationDataList[InformationId];
        Information information = new Information(baseData);
        return information;
    }

    public List<Information> GetActiveInformationsByTime(DateTime targetTime)
    {
        List<Information> activeInformations = new List<Information>();
        List<Item> playerItems = PlayerController.Instance.GetItemList();

        for (int i = 0; i < InformationDataList.Count; i++)
        {
            InformationBase informationBase = InformationDataList[i];
            if (IsActiveDateTime(informationBase, targetTime))
            {
                Information information = new Information(informationBase);
                activeInformations.Add(information);
            }
        }
        return activeInformations;
    }

    public List<Information> GetActiveInformationByField(DateTime targetTime, FieldType fieldType)
    {
        List<Information> activeInformations = new List<Information>();
        List<Item> playerItems = PlayerController.Instance.GetItemList();

        for (int i = 0; i < InformationDataList.Count; i++)
        {
            InformationBase informationBase = InformationDataList[i];
            if (informationBase.FieldType != fieldType)
            {
                continue; // フィールドタイプが一致しない場合はスキップ
            }
            if (IsActiveDateTime(informationBase, targetTime))
            {
                Information information = new Information(informationBase);
                activeInformations.Add(information);
            }
        }
        return activeInformations;
    }

    public List<Information> GetActiveInformationsByPoint(DateTime targetTime, PointBase pointBase)
    {
        List<Information> activeInformations = new List<Information>();
        List<Item> playerItems = PlayerController.Instance.GetItemList();

        for (int i = 0; i < InformationDataList.Count; i++)
        {
            InformationBase informationBase = InformationDataList[i];
            if (informationBase.PointBase != pointBase)
            {
                continue; // ポイントが一致しない場合はスキップ
            }
            if (IsActiveDateTime(informationBase, targetTime))
            {
                Information information = new Information(informationBase);
                activeInformations.Add(information);
            }
        }
        return activeInformations;
    }

    private bool IsActiveDateTime(InformationBase informationBase, DateTime targetTime)
    {
        // 年と月が一致しているものを有効とする
        if (targetTime == informationBase.StartDateTime)
        {
            return true;
        }
        return false;
    }

    public int GetInformationId(InformationBase Information)
    {
        if (Information == null)
        {
            return -1;
        }
        return InformationDataList.IndexOf(Information);
    }

    public void MarkInformationAsFinished(InformationBase informationBase)
    {
        int informationId = GetInformationId(informationBase);
        if (informationId == -1)
        {
            UnityEngine.Debug.LogWarning("Attempted to mark an invalid information as finished.");
            return;
        }
        MarkInformationAsFinishedById(informationId);
    }

    public void MarkInformationAsFinishedById(int informationId)
    {
        if (ExecutionInformations.Contains(new Vector2Int(informationId, 0)))
        {
            int index = ExecutionInformations.IndexOf(new Vector2Int(informationId, 0));
            ExecutionInformations[index] = new Vector2Int(informationId, ExecutionInformations[index].y + 1);
        }
        else
        {
            ExecutionInformations.Add(new Vector2Int(informationId, 1));
        }
    }
}