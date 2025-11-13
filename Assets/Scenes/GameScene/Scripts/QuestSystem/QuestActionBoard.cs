using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// TODO QuestPanelControllerに名前変更
public class QuestActionBoard : SlidePanel
{
    public UnityAction OnQuestEnd;

    [SerializeField] private QuestCardPrefab questCardPrefab;
    [SerializeField] private ActionIcon questIconPrefab;
    [SerializeField] private ActionIcon quitIconPrefab;
    [SerializeField] private StatusText statusText;
    [SerializeField] private GameObject actionIconArea;
    [SerializeField] private GameObject questCardArea;

    private int currentIndex = 0;
    private int panelCount = 0;
    private List<ActionIcon> actionIconList = new List<ActionIcon>();
    private List<QuestCardPrefab> questCardList = new List<QuestCardPrefab>();

    public delegate void TargetItemDelegate(Item item, bool isOwn = true);
    public event TargetItemDelegate OnTargetItem;

    public void SetQuest(List<Quest> quests)
    {
        currentIndex = 0;
        panelCount = quests.Count; // クエスト数 + 終了アイコン（0スタートなので+1はしない）
        questCardList = new List<QuestCardPrefab>();
        questCardArea.transform.DetachChildren();
        SetActionIcons();
        SetActionPanels(quests);
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
            if (currentIndex == panelCount) // 終了アイコンが選択されている場合
            {
                OnQuestEnd?.Invoke(); // 予約終了イベントを呼び出す
            }
        }
    }

    private void ChoiceAction(bool isLeft)
    {
        // actionPanelsから現在のアクションのインデックスを取得
        if (isLeft)
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex = panelCount;
            }
        }
        else
        {
            currentIndex++;
            if (currentIndex > panelCount)
            {
                currentIndex = 0;
            }
        }
        ChangeActiveIcon();
        ChangeActionPanel();
    }

    private void SetActionPanels(List<Quest> quests)
    {
        foreach (var card in questCardList)
        {
            Destroy(card.gameObject);
        }
        questCardList.Clear();
        foreach (var quest in quests)
        {
            QuestCardPrefab card = Instantiate(questCardPrefab, questCardArea.transform);
            quest.Init();
            card.SetQuest(quest);
            card.OnTargetItem += TargetItem;
            questCardList.Add(card);
        }
    }

    private void TargetItem(Item item, bool isOwn = true)
    {
        OnTargetItem?.Invoke(item, isOwn);
    }

    private void SetActionIcons()
    {
        actionIconList.Clear();
        GameObject[] objects = GameObject.FindGameObjectsWithTag("ActionIcon");
        foreach (var obj in objects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        actionIconArea.transform.DetachChildren();
        for (int i = 0; i < panelCount; i++)
        {
            ActionIcon icon = Instantiate(questIconPrefab, actionIconArea.transform);
            actionIconList.Add(icon);
        }
        ActionIcon quitIcon = Instantiate(quitIconPrefab, actionIconArea.transform);
        actionIconList.Add(quitIcon);
    }

    private void ChangeActiveIcon()
    {
        for (int i = 0; i < actionIconList.Count; i++)
        {
            actionIconList[i].SetActive(i == currentIndex);
        }
        if (currentIndex == panelCount)
        {
            statusText.SetText("Quit");
        }
        else
        {
            statusText.SetText("Quest");
        }
    }

    private void ChangeActionPanel()
    {
        for (int i = 0; i < questCardList.Count; i++)
        {
            questCardList[i].gameObject.SetActive(i == currentIndex);
        }
        // 終了アイコンが選択されている場合、クエストカードを非表示にする
        if (currentIndex == panelCount)
        {
            foreach (var questCard in questCardList)
            {
                questCard.gameObject.SetActive(false);
            }
        }
    }

    public void ClosePanel(Action onComplete = null)
    {
        int completed = 0;
        int completeIndex = 1;


        if (currentIndex < questCardList.Count)
        {
            completeIndex = 2;
        }
        void CheckAllComplete()
        {
            completed++;
            if (completed >= completeIndex)
            {
                currentIndex = 0;
                onComplete?.Invoke();
            }
        }
        if (currentIndex < questCardList.Count)
        {
            questCardList[currentIndex].SetActive(false, CheckAllComplete);
        }
        this.SetActive(false, CheckAllComplete);
    }
}
