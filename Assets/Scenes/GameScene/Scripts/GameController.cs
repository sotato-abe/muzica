using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private AgeTimePanel ageTimePanel;

    private void Start()
    {
        // ゲーム開始時にエージェントの初期化を行う
        ageTimePanel.SetTimeSpeed(TimeState.Fast);  // 初期状態をFastに設定
    }
}
