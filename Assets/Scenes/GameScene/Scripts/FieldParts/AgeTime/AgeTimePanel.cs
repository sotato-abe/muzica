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

    private DateTime ageTime;        // 現在の時間
    public TimeState timeSpeed = TimeState.Fast;

    private void Start()
    {
        SetTimeSpeed(TimeState.Fast);  // 初期状態をFastに設定
        Init();
    }

    // 初期化して現在の時間を設定
    public void Init()
    {
        ageTime = new DateTime(2030, 12, 1);  // 初期値を2030年12月に設定
        UpdateAgeTimeField();
    }

    // TimeStateを変化させる。
    public void SetTimeSpeed(TimeState state)
    {
        timeSpeed = state;
        statePanel.ChangeState(state);
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

        UpdateAgeTimeField();
    }

    // ageTimeFieldに時間を表示
    private void UpdateAgeTimeField()
    {
        ageTimeField.text = ageTime.ToString("yyyy/MM/dd");
    }
}
