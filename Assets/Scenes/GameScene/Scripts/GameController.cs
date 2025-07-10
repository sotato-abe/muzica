using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private AgeTimePanel ageTimePanel;
    [SerializeField] ReserveSystem reserveSystem;
    [SerializeField] TradeSystem tradeSystem;
    [SerializeField] FieldPlayer fieldPlayer;
    [SerializeField] FieldController fieldController;

    private void Start()
    {
        // ゲーム開始時にエージェントの初期化を行う
        ageTimePanel.SetTimeSpeed(TimeState.Fast);  // 初期状態をFastに設定
        fieldPlayer.OnReserveStart += ReserveStart; // リザーブ開始イベントを登録
        reserveSystem.OnReserveEnd += ReserveEnd; // リザーブ終了イベントを登録
        tradeSystem.OnTradeEnd += TradeEnd; // リザーブ終了イベントを登録
        fieldController.OnPointEnter += TradeStart;
    }

    public void ReserveStart()
    {
        reserveSystem.gameObject.SetActive(true); // リザーブシステムを非表示にする
        ageTimePanel.SetTimeSpeed(TimeState.Live);
        reserveSystem.ReserveStart(); // リザーブ開始処理を呼び出す
    }

    public void ReserveEnd()
    {
        reserveSystem.gameObject.SetActive(false); // リザーブシステムを非表示にする
        ageTimePanel.SetTimeSpeed(TimeState.Fast);
        fieldPlayer.SetCanMove(true); // プレイヤーの移動を再開
    }

    public void TradeStart(PointBase point)
    {
        tradeSystem.gameObject.SetActive(true); // リザーブシステムを非表示にする
        ageTimePanel.SetTimeSpeed(TimeState.Live);
        tradeSystem.TradeStart(point); // リザーブ開始処理を呼び出す
        fieldPlayer.SetCanMove(false); // プレイヤーの移動を再開
    }

    public void TradeEnd()
    {
        tradeSystem.gameObject.SetActive(false); // リザーブシステムを非表示にする
        ageTimePanel.SetTimeSpeed(TimeState.Fast);
        fieldPlayer.SetCanMove(true); // プレイヤーの移動を再開
    }
}
