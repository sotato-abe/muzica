using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class QuestController : MonoBehaviour
{
    public static QuestController Instance { get; private set; }

    [SerializeField] MessagePanel messagePanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void EnterQuest(Quest quest)
    {
        if (quest == null)
        {
            Debug.LogWarning("Quest is null. Cannot enter quest.");
            return;
        }

        // メッセージパネルにメッセージを表示
        messagePanel.AddMessage(MessageIconType.Quest, $"{quest.Base.Title} を発見");
    }
}
