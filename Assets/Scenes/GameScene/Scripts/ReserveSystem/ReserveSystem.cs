using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReserveSystem : MonoBehaviour
{
    public UnityAction OnReserveEnd;

    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private ReserveActionBoard reserveActionBoard; // リザーブアクションボード
    [SerializeField] SlidePanel footer;

    // コンポーネント外
    [SerializeField] private CharacterSubPanel playerSubPanel; // キャラクターサブパネル
    [SerializeField] private MessagePanel messagePanel; // キャラクターサブパネル
    [SerializeField] WorldMapPanel worldMapPanel;
    [SerializeField] SlidePanel optionActionBoard;

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
        if (PlayerController.Instance.CurrentEventType != EventType.Default)
            return;
        PlayerController.Instance.ChangeEventType(EventType.Reserve); // イベントタイプをリザーブに変更
        SoundSystem.Instance.PlaySE(SeType.PanelOpen); // SEをオープンに変更

        cameraManager.SetEventType(EventType.Reserve); // 準備時のカメラ位置を設定
        worldMapPanel.SetActive(false); // ワールドマップパネルを非表示
        optionActionBoard.SetActive(false); // オプションボタンリストを非表示

        footer.SetActive(true); // フッターを非表示にする
        playerSubPanel.SetActive(true); // キャラクターサブパネルを表示
        reserveActionBoard.SetActive(true); // リザーブアクションボードを表示
        TalkMessage talkMessage = new TalkMessage(MessageType.Talk, MessagePanelType.Default, "準備しよう");
        StartCoroutine(playerSubPanel.SetTalkMessage(talkMessage)); // リザーブアクションボードを開く
    }

    public void ResorveEnd()
    {
        int completed = 0;
        SoundSystem.Instance.PlaySE(SeType.PanelClose);
        void CheckAllComplete()
        {
            completed++;
            if (completed >= 5)
            {
                OnReserveEnd?.Invoke();
                transform.gameObject.SetActive(false);
            }
        }
        reserveActionBoard.WindowClose(CheckAllComplete); // リザーブアクションボードを非表示にする
        footer.SetActive(false, CheckAllComplete); // フッターを非表示にする
        playerSubPanel.SetActive(false, CheckAllComplete); // キャラクターサブパネルを非表示にする

        worldMapPanel.SetActive(true, CheckAllComplete); // ワールドマップパネルを表示
        optionActionBoard.SetActive(true, CheckAllComplete); // オプションボタンリストを表示
        cameraManager.SetEventType(EventType.Default); // 通常時のカメラ位置を設定
        PlayerController.Instance.ChangeEventType(EventType.Default); // イベントタイプをデフォルトに変更
    }
}
