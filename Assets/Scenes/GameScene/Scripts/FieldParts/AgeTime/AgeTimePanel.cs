using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class AgeTimePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ageTimeField;  // 表示用のTextMeshProUGUIフィールド
    [SerializeField] StatePanelController statePanel;
    [SerializeField] PointDatabase pointDatabase;

    public int yearsElapsed = 0;        // 経過時間
    public DateTime ageTime;        // 現在の時間
    public TimeState timeSpeed = TimeState.Fast;
    private int lastYear; // 直前の年を記録

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

        UpdateAgeTimeField();
    }

    // ageTimeFieldに時間を表示
    private void UpdateAgeTimeField()
    {
        ageTimeField.text = ageTime.ToString("yyyy/MM");
    }
}
