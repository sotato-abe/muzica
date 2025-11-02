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

    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private QuestActionBoard questActionBoard; // クエストアクションボード
    [SerializeField] TalkPanel talkPanel;

    [SerializeField] WorldMapPanel worldMapPanel;
    [SerializeField] SlidePanel savePanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        questActionBoard.OnQuestEnd += QuestEnd;
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

        cameraManager.SetEventType(EventType.Trade); // トレード時のカメラ位置を設定
        worldMapPanel.SetActive(false); // ワールドマップパネルを非表示
        savePanel.SetActive(false); // セーブパネルを非表示

        talkPanel.SetQuest(quest);
        questActionBoard.SetActive(true); // クエストアクションボードを表示
    }

    public void QuestEnd()
    {
        int completed = 0;
        SoundSystem.Instance.PlaySE(SeType.PanelClose);
        void CheckAllComplete()
        {
            completed++;
            if (completed >= 3)
            {
                OnQuestEnd?.Invoke();
                transform.gameObject.SetActive(false);
            }
        }
        questActionBoard.ClosePanel(CheckAllComplete); // クエストアクションボードを表示
        worldMapPanel.SetActive(true, CheckAllComplete); // ワールドマップパネルを表示
        savePanel.SetActive(true, CheckAllComplete); // セーブパネルを表示
        cameraManager.SetEventType(EventType.Default); // バトル時のカメラ位置を設定
    }
}
