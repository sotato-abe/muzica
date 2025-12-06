using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class AgeTimePanel : MonoBehaviour
{
    public static AgeTimePanel Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI ageTimeField;  // 表示用のTextMeshProUGUIフィールド
    [SerializeField] private TextMeshProUGUI ageTimeBackField;  // 表示用のTextMeshProUGUIフィールド
    [SerializeField] StatePanelController statePanel;
    [SerializeField] PointDatabase pointDatabase;

    public int yearsElapsed = 0;        // 経過時間
    public DateTime ageTime;        // 現在の時間
    public TimeState timeSpeed = TimeState.Fast;
    private int lastYear; // 直前の年を記録
    private int lastMonth; // 直前の月を記録

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        SetTimeSpeed(TimeState.Fast);
    }

    public void SetTimeSpeed(TimeState state)
    {
        timeSpeed = state;
        statePanel.ChangeState(state);
    }

    public void TimeSlip(DateTime targetTime)
    {
        ageTime = targetTime;
        lastYear = ageTime.Year;
        lastMonth = ageTime.Month;
        SoundSystem.Instance.PlaySE(SeType.TimeSlip);
        UpdateAgeTimeField();
    }

    // 時間経過を管理し、timeSpeedに応じて進行速度を変更する
    private void Update()
    {
        if (timeSpeed == TimeState.Stop) return;

        float deltaTime = Time.deltaTime;

        switch (timeSpeed)
        {
            case TimeState.Live:
                ageTime = ageTime.AddSeconds(deltaTime);  // 通常速度で秒単位で進行
                break;
            case TimeState.Fast:
                ageTime = ageTime.AddDays(deltaTime * (10957.5f / 3600f)); // 30年60分
                break;
        }
        if (ageTime.Year != lastYear)
        {
            yearsElapsed++;
            lastYear = ageTime.Year;
            pointDatabase.ResetMerchandise();
        }
        // 月が更新されるタイミングでUpdateAgeTimeFieldを呼び出す
        if (ageTime.Month != lastMonth)
        {
            lastMonth = ageTime.Month;
            CheckInformation();
            UpdateAgeTimeField();
        }
    }

    public void PassageOfMonth(int months)
    {
        SoundSystem.Instance.PlaySE(SeType.TimeSlip);
        StartCoroutine(PassageOfTime(months));
    }

    private IEnumerator PassageOfTime(int months)
    {
        // 現在時刻からからageTimeごとに0.3秒かけて加算して表示を更新する
        for (int i = 0; i < months; i++)
        {
            ageTime = ageTime.AddMonths(1);
            UpdateAgeTimeField();
            yield return new WaitForSeconds(0.3f);
        }
        lastYear = ageTime.Year;
        lastMonth = ageTime.Month;
    }

    // ageTimeFieldに時間を表示
    private void UpdateAgeTimeField()
    {
        ageTimeField.text = ageTime.ToString("yyyy/MM");
        ageTimeBackField.text = ageTime.ToString("yyyy/MM");
    }

    private void CheckInformation()
    {
        DateTime targetTime = new DateTime(ageTime.Year, ageTime.Month, 1);
        List<Information> activeInformations = InformationDatabase.Instance.GetActiveInformationsByTime(targetTime);
        foreach (Information information in activeInformations)
        {
            InformationPanel.Instance.SetInformation(information);
        }
    }
}
