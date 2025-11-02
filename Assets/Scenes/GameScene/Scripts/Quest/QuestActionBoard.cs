using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// BattlePanelとIconの表示と切り替えを管理するクラス
public class QuestActionBoard : SlidePanel
{
    public UnityAction OnQuestEnd;

    [SerializeField] private TalkPanel talkPanel;

    [SerializeField] private ActionIcon talkIcon;
    [SerializeField] private ActionIcon quitIcon;
    [SerializeField] private StatusText statusText;

    private Dictionary<QuestActionType, TwoColumnPanel> actionPanels;
    private Dictionary<QuestActionType, ActionIcon> actionIcons;
    private List<QuestActionType> actionTypeList;
    private QuestActionType currentAction = QuestActionType.Talk;

    private bool isTalkPanelActive = true;

    private void Start()
    {
        actionPanels = new Dictionary<QuestActionType, TwoColumnPanel>
        {
            {  QuestActionType.Talk, talkPanel },
        };

        actionIcons = new Dictionary<QuestActionType, ActionIcon>
        {
            {  QuestActionType.Talk, talkIcon },
            {  QuestActionType.Quit, quitIcon },
        };

        actionTypeList = new List<QuestActionType>(actionIcons.Keys);

        ChangeActiveIcon();
        ChangeActionPanel();
    }

    private void OnEnable()
    {
        if(actionPanels == null || actionIcons == null || actionTypeList == null)
            return;
        currentAction = QuestActionType.Talk;
        ChangeActiveIcon();
        ChangeActionPanel();
    }

    private void Update()
    {
        if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ChoiceAction(false);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ChoiceAction(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (currentAction == QuestActionType.Quit)
            {
                UnityEngine.Debug.Log("QuestActionBoard: Quit quest action selected.");
                OnQuestEnd?.Invoke(); // 予約終了イベントを呼び出す
            }
        }
    }

    private void ChoiceAction(bool isLeft)
    {
        // actionPanelsから現在のアクションのインデックスを取得
        int currentIndex = actionTypeList.IndexOf(currentAction);
        if (isLeft)
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex = actionTypeList.Count - 1;
            }
        }
        else
        {
            currentIndex++;
            if (currentIndex >= actionTypeList.Count)
            {
                currentIndex = 0;
            }
        }
        currentAction = actionTypeList[currentIndex];
        ChangeActiveIcon();
        ChangeActionPanel();
    }

    public void TalkPanelOpen()
    {
        ChangeAction(QuestActionType.Talk);
    }

    public void QuitQuest()
    {
        OnQuestEnd?.Invoke();
    }

    public void ChangeAction(QuestActionType actionType)
    {
        currentAction = actionType;
        ChangeActiveIcon();
        ChangeActionPanel();
    }

    private void ChangeActiveIcon()
    {
        foreach (var kvp in actionIcons)
        {
            kvp.Value.SetActive(kvp.Key == currentAction); // 選択状態を表示
        }
    }

    private void ChangeActionPanel()
    {
        SoundSystem.Instance.PlaySE(SeType.Select);
        if (currentAction == QuestActionType.Quit)
        {
            statusText.SetText(currentAction.GetActionText()); // ステータステキストを更新
            return;
        }

        foreach (var kvp in actionPanels)
        {
            if (kvp.Key == currentAction)
            {
                kvp.Value.gameObject.SetActive(true);
                kvp.Value.SetActive(true);
                statusText.SetText(kvp.Key.GetActionText()); // ステータステキストを更新
            }
            else
            {
                kvp.Value.SetActive(false);
            }
        }
    }

    public void ClosePanel(Action onComplete = null)
    {
        talkPanel.SetActive(false);
        this.SetActive(false);
        onComplete?.Invoke();
    }
}
