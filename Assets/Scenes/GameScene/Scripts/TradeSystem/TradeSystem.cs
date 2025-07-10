using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TradeSystem : MonoBehaviour
{
    public UnityAction OnTradeEnd; // リザーブイベント

    // [SerializeField] private TradeActionBoard tradeActionBoard; // リザーブアクションボード
    [SerializeField] private CharacterSubPanel leftcharacterSubPanel; // キャラクターサブパネル
    [SerializeField] private CharacterSubPanel rightcharacterSubPanel; // キャラクターサブパネル
    [SerializeField] private MessagePanel messagePanel; // キャラクターサブパネル
    [SerializeField] WorldMapPanel worldMapPanel;

    private PointBase currentPoint;

    private void Start()
    {
        // tradeActionBoard.OnTradeEnd += TradeEnd; // リザーブアクションボードの終了イベントを登録
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
        {
            TradeEnd();
        }
    }

    public void TradeStart(PointBase point)
    {
        currentPoint = point; // 現在のポイントを保存
        // tradeActionBoard.SetActive(true); // リザーブアクションボードを表示
        TalkMessage talkMessage1 = new TalkMessage(MessageType.Talk, MessagePanelType.Default, "いいものあるかな");
        TalkMessage talkMessage2 = new TalkMessage(MessageType.Talk, MessagePanelType.Default, "いらっしゃい");
        StartCoroutine(leftcharacterSubPanel.SetTalkMessage(talkMessage1, true));
        rightcharacterSubPanel.SetCharacter(point.Owner.Base.SquareSprite);
        StartCoroutine(rightcharacterSubPanel.SetTalkMessage(talkMessage2, true));
        messagePanel.SetActive(false); // メッセージパネルを表示
        worldMapPanel.SetActive(false); // ワールドマップパネルを非表示
    }

    public void TradeEnd()
    {
        int completed = 0;
        void CheckAllComplete()
        {
            completed++;
            if (completed >= 5)
            {
                OnTradeEnd?.Invoke();
                transform.gameObject.SetActive(false);
            }
        }

        // tradeActionBoard.SetActive(false, CheckAllComplete); // リザーブアクションボードを表示
        leftcharacterSubPanel.SetActive(false, CheckAllComplete); // キャラクターサブパネルを表示
        rightcharacterSubPanel.SetActive(false, CheckAllComplete); // キャラクターサブパネルを表示
        messagePanel.SetActive(true, CheckAllComplete); // メッセージパネルを表示
        worldMapPanel.SetActive(true, CheckAllComplete); // ワールドマップパネルを表示
    }
}
