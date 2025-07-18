using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReserveSystem : MonoBehaviour
{
    public UnityAction OnReserveEnd;

    [SerializeField] private ReserveActionBoard reserveActionBoard; // リザーブアクションボード
    [SerializeField] SlidePanel footer;

    // コンポーネント外
    [SerializeField] private CharacterSubPanel playerSubPanel; // キャラクターサブパネル
    [SerializeField] private MessagePanel messagePanel; // キャラクターサブパネル
    [SerializeField] WorldMapPanel worldMapPanel;

    private void Start()
    {
        reserveActionBoard.OnReserveEnd += ResorveEnd; // リザーブアクションボードの終了イベントを登録
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
        {
            ResorveEnd();
        }
    }

    public void ReserveStart()
    {
        footer.SetActive(true); // フッターを非表示にする
        playerSubPanel.SetActive(true); // キャラクターサブパネルを表示
        TalkMessage talkMessage = new TalkMessage(MessageType.Talk, MessagePanelType.Default, "準備しよう");
        StartCoroutine(playerSubPanel.SetTalkMessage(talkMessage)); // リザーブアクションボードを開く
        messagePanel.SetActive(false); // メッセージパネルを表示
        worldMapPanel.SetActive(false); // ワールドマップパネルを非表示
    }

    public void ResorveEnd()
    {
        int completed = 0;
        void CheckAllComplete()
        {
            completed++;
            if (completed >= 4)
            {
                OnReserveEnd?.Invoke();
                transform.gameObject.SetActive(false);
            }
        }

        footer.SetActive(false, CheckAllComplete); // フッターを非表示にする
        playerSubPanel.SetActive(false, CheckAllComplete); // キャラクターサブパネルを表示
        messagePanel.SetActive(true, CheckAllComplete); // メッセージパネルを表示
        worldMapPanel.SetActive(true, CheckAllComplete); // ワールドマップパネルを表示
    }
}
