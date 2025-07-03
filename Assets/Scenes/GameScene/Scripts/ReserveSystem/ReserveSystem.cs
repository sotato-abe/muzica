using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReserveSystem : MonoBehaviour
{
    public UnityAction OnReserveEnd; // リザーブイベント

    [SerializeField] private ReserveActionBoard reserveActionBoard; // リザーブアクションボード
    [SerializeField] private CharacterSubPanel characterSubPanel; // キャラクターサブパネル
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
        Debug.Log("ReserveStart");
        reserveActionBoard.SetActive(true); // リザーブアクションボードを表示
        // characterSubPanel.SetActive(true); // キャラクターサブパネルを非表示
        TalkMessage talkMessage = new TalkMessage(MessageType.Talk, MessagePanelType.Default, "準備しよう");
        StartCoroutine(characterSubPanel.SetTalkMessage(talkMessage, true)); // リザーブアクションボードを開く
        messagePanel.SetActive(false); // メッセージパネルを表示
        worldMapPanel.SetActive(false); // ワールドマップパネルを非表示
    }

    public void ResorveEnd()
    {
        Debug.Log("ResorveEnd");
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

        reserveActionBoard.SetActive(false, CheckAllComplete); // リザーブアクションボードを表示
        characterSubPanel.SetActive(false, CheckAllComplete); // キャラクターサブパネルを表示
        messagePanel.SetActive(true, CheckAllComplete); // メッセージパネルを表示
        worldMapPanel.SetActive(true, CheckAllComplete); // ワールドマップパネルを表示
    }
}
