using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    [SerializeField] private AgeTimePanel ageTimePanel;
    [SerializeField] ReserveSystem reserveSystem;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] TradeSystem tradeSystem;
    [SerializeField] FieldPlayer fieldPlayer;
    [SerializeField] FieldController fieldController;
    [SerializeField] SaveManagement saveManagement;
    [SerializeField] private PlayerCharacter defaultPlayer; // テスト用


    private void Start()
    {
        fieldPlayer.OnReserveStart += ReserveStart; // リザーブ開始イベントを登録
        fieldPlayer.OnBattleStart += BattleStart; // バトル開始イベントを登録
        reserveSystem.OnReserveEnd += ReserveEnd; // リザーブ終了イベントを登録
        battleSystem.OnBattleEnd += BattleEnd; // バトル終了イベントを登録
        tradeSystem.OnTradeEnd += TradeEnd; // リザーブ終了イベントを登録
        fieldController.OnPointEnter += TradeStart;
        LoadPlayData();
    }

    private void LoadPlayData()
    {
        PlayData selectedPlayData = GameScene.selectedPlayData;
        if (selectedPlayData == null)
        {
            selectedPlayData = new PlayData();
            defaultPlayer.Init();
            selectedPlayData.playerData = saveManagement.PlayerDataConverter(defaultPlayer);
            selectedPlayData.position = defaultPlayer.Base.Birthplace.Position;
            selectedPlayData.time = new DateTime(2030, 4, 1);
            GameScene.selectedPlayData = selectedPlayData;
        }
        ageTimePanel.TimeSlip(selectedPlayData.time);
        WorldMapController.Instance.WarpPlayerCoordinate(selectedPlayData.position);
        SoundSystem.Instance.PlayBGM(BgmType.Field);
        fieldPlayer.SetCanMove(true);
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
    public void BattleStart()
    {
        battleSystem.gameObject.SetActive(true); // リザーブシステムを非表示にする
        ageTimePanel.SetTimeSpeed(TimeState.Live);
        battleSystem.BattleStart(); // リザーブ開始処理を呼び出す
    }

    public void BattleEnd()
    {
        battleSystem.gameObject.SetActive(false); // リザーブシステムを非表示にする
        ageTimePanel.SetTimeSpeed(TimeState.Fast);
        fieldPlayer.SetCanMove(true); // プレイヤーの移動を再開
    }

    public void TradeStart(Point point)
    {
        tradeSystem.gameObject.SetActive(true); // リザーブシステムを非表示にする
        ageTimePanel.SetTimeSpeed(TimeState.Live);
        tradeSystem.TradeStart(point); // リザーブ開始処理を呼び出す
        fieldPlayer.SetCanMove(false); // プレイヤーの移動を停止
    }

    public void TradeEnd()
    {
        tradeSystem.gameObject.SetActive(false); // リザーブシステムを非表示にする
        ageTimePanel.SetTimeSpeed(TimeState.Fast);
        fieldPlayer.SetCanMove(true); // プレイヤーの移動を再開
    }
}
