using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleSystem : MonoBehaviour
{
    public UnityAction OnBattleEnd;

    [SerializeField] private BattleActionBoard battleActionBoard;
    [SerializeField] private CharacterSubPanel playerSubPanel; // キャラクターサブパネル
    [SerializeField] private CharacterSubPanel enemySubPanel; // キャラクターサブパネル
    [SerializeField] private MessagePanel messagePanel; // キャラクターサブパネル
    [SerializeField] WorldMapPanel worldMapPanel;

    private void Start()
    {
        battleActionBoard.OnBattleEnd += BattleEnd; // リザーブアクションボードの終了イベントを登録
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
        {
            BattleEnd();
        }
    }

    public void BattleStart()
    {
        battleActionBoard.SetActive(true); // リザーブアクションボードを表示
        TalkMessage talkMessage = new TalkMessage(MessageType.Talk, MessagePanelType.Default, "なんだよ");
        playerSubPanel.SetActive(true); // キャラクターサブパネルを表示
        StartCoroutine(playerSubPanel.SetTalkMessage(talkMessage)); // リザーブアクションボードを開く
        messagePanel.SetActive(false); // メッセージパネルを表示
        worldMapPanel.SetActive(false); // ワールドマップパネルを非表示
        SetEnemy();
    }

    private void SetEnemy()
    {
        List<Character> enemies = FieldController.Instance.GetEnemies();
        enemySubPanel.SetCharacter(enemies[0]);
        enemySubPanel.SetActive(true); // キャラクターサブパネルを表示
    }

    public void BattleEnd()
    {
        int completed = 0;
        void CheckAllComplete()
        {
            completed++;
            if (completed >= 5)
            {
                OnBattleEnd?.Invoke();
                transform.gameObject.SetActive(false);
            }
        }

        battleActionBoard.SetActive(false, CheckAllComplete); // リザーブアクションボードを表示
        playerSubPanel.SetActive(false, CheckAllComplete); // キャラクターサブパネルを表示
        enemySubPanel.SetActive(false, CheckAllComplete); // キャラクターサブパネルを表示
        messagePanel.SetActive(true, CheckAllComplete); // メッセージパネルを表示
        worldMapPanel.SetActive(true, CheckAllComplete); // ワールドマップパネルを表示
    }
}
