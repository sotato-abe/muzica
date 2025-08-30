using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TradeSystem : MonoBehaviour
{
    public UnityAction OnTradeEnd; // リザーブイベント
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private TradeActionBoard tradeActionBoard; // リザーブアクションボード
    [SerializeField] private CharacterSubPanel playerSubPanel; // キャラクターサブパネル
    [SerializeField] private CharacterSubPanel ownerSubPanel; // キャラクターサブパネル
    [SerializeField] private MessagePanel messagePanel; // キャラクターサブパネル
    [SerializeField] ItemTradePanel itemTradePanel;
    [SerializeField] CommandTradePanel commandTradePanel;
    [SerializeField] WorldMapPanel worldMapPanel;
    [SerializeField] SlidePanel savePanel;

    private Point currentPoint;

    private void Start()
    {
        tradeActionBoard.OnTradeEnd += TradeEnd; // リザーブアクションボードの終了イベントを登録
        itemTradePanel.OnOwnerMessage += SetOwnerMessage;
        commandTradePanel.OnOwnerMessage += SetOwnerMessage;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
        {
            TradeEnd();
        }
    }

    public void TradeStart(Point point)
    {
        if (PlayerController.Instance.CurrentEventType != EventType.Default)
            return;

        PlayerController.Instance.ChangeEventType(EventType.Trade); // イベントタイプをトレードに変更
        cameraManager.SetEventType(EventType.Trade); // トレード時のカメラ位置を設定
        messagePanel.SetActive(false); // メッセージパネルを表示
        worldMapPanel.SetActive(false); // ワールドマップパネルを非表示
        savePanel.SetActive(false); // セーブパネルを非表示

        playerSubPanel.SetActive(true); // キャラクターサブパネルを表示
        playerSubPanel.SetMessageByType(MessageType.Entrance); // プレイヤーのメッセージを設定
        SetPoint(point); // 現在のポイントを設定
        tradeActionBoard.SetActive(true); // リザーブアクションボードを表示
    }

    private void SetPoint(Point point)
    {
        currentPoint = point; // 現在のポイントを保存
        itemTradePanel.SetPoint(point); // アイテムトレードパネルにポイントを設定
        commandTradePanel.SetPoint(point); // コマンドトレードパネルにポイントを設定
        Character owner = point.Owner;
        owner.Init();
        ownerSubPanel.SetOwner(owner);
    }

    public void SetOwnerMessage(TalkMessage message)
    {
        StartCoroutine(ownerSubPanel.SetTalkMessage(message));
    }

    public void TradeEnd()
    {
        int completed = 0;
        void CheckAllComplete()
        {
            completed++;
            if (completed >= 6)
            {
                OnTradeEnd?.Invoke();
                transform.gameObject.SetActive(false);
            }
        }

        tradeActionBoard.SetActive(false, CheckAllComplete); // リザーブアクションボードを表示
        playerSubPanel.SetActive(false, CheckAllComplete); // キャラクターサブパネルを表示
        ownerSubPanel.SetActive(false, CheckAllComplete); // キャラクターサブパネルを表示
        messagePanel.SetActive(true, CheckAllComplete); // メッセージパネルを表示
        worldMapPanel.SetActive(true, CheckAllComplete); // ワールドマップパネルを表示
        savePanel.SetActive(true, CheckAllComplete); // セーブパネルを表示
        cameraManager.SetEventType(EventType.Default); // バトル時のカメラ位置を設定
        PlayerController.Instance.ChangeEventType(EventType.Default); // イベントタイプをデフォルトに変更
    }
}
