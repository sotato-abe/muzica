using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class QuestSystem : MonoBehaviour
{
    public UnityAction OnQuestEnd; // クエストイベント
    
    public static QuestSystem Instance { get; private set; }

    [SerializeField] MessagePanel messagePanel;
    [SerializeField] WorldMapPanel worldMapPanel;
    [SerializeField] TalkPanel talkPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
        {
            QuestEnd();
        }
    }

    public void QuestStart(Quest quest)
    {
        if (quest == null)
        {
            Debug.LogWarning("Quest is null. Cannot enter quest.");
            return;
        }

        // メッセージパネルにメッセージを表示
        worldMapPanel.SetActive(false); // ワールドマップパネルを非表示
        talkPanel.SetQuest(quest);
        talkPanel.SetActive(true);
        messagePanel.AddMessage(MessageIconType.Quest, $"{quest.Base.Title} を発見");
    }

    public void QuestEnd()
    {
        int completed = 0;
        SoundSystem.Instance.PlaySE(SeType.PanelClose);
        void CheckAllComplete()
        {
            completed++;
            if (completed >= 1)
            {
                OnQuestEnd?.Invoke();
                transform.gameObject.SetActive(false);
            }
        }
        worldMapPanel.SetActive(true, CheckAllComplete); // ワールドマップパネルを表示
    }
}
