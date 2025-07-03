using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    [SerializeField] private CharacterSubPanel playerSubPanel;
    [SerializeField] private PlayerCharacter player;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 重複防止
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        player.Init();  // プレイヤーキャラクターの初期化
        Debug.Log($"PlayerController Start: {player.Base.Name} initialized with Level {player.Level} and Exp {player.Exp}");
        StartCoroutine(GameStart());
    }

    private IEnumerator GameStart()
    {
        yield return new WaitForSeconds(1f);
        TalkMessage startMessage = new TalkMessage(MessageType.Talk, MessagePanelType.Default, "はじめるか、、");
        StartCoroutine(playerSubPanel.SetTalkMessage(startMessage));
    }

    public void SetPlayerBlowing(TalkMessage talkMessage)
    {
        playerSubPanel.SetTalkMessage(talkMessage);
    }
}
