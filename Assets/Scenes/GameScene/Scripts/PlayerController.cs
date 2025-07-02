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
        StartCoroutine(SetPlayerBlowing(startMessage));
    }

    public IEnumerator SetPlayerBlowing(TalkMessage talkMessage)
    {
        if (playerSubPanel != null)
        {
            playerSubPanel.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            playerSubPanel.SetTalkMessage(talkMessage);
            yield return new WaitForSeconds(3.0f);
            playerSubPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("PlayerSubPanel is not assigned in PlayerController.");
        }
    }
}
