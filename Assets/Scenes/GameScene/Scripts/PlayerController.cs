using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    [SerializeField] private CharacterSubPanel playerSubPanel;
    [SerializeField] private PlayerCharacter player;
    public PlayerCharacter PlayerCharacter => player;

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

    public void AddItemToBag(Item item)
    {
        // バッグの容量を超える場合はアイテムを入手しない
        if (player.BagItemList.Count >= player.Bag)
        {
            Debug.LogWarning("バッグの容量を超えています。アイテムを追加できません。");
            return;
        }
        player.AddItem(item);
    }

    public void DropItemFromBag(Item item)
    {
        if (item == null) return;
        FieldController.Instance.DropPlayerItem(item);
        // アイテムをドロップする処理
        PlayerCharacter.RemoveItem(item);
    }
}
