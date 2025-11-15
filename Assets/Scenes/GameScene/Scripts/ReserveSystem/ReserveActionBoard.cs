using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// BattlePanelとIconの表示と切り替えを管理するクラス
public class ReserveActionBoard : SlidePanel
{
    public UnityAction OnReserveEnd;
    [SerializeField] private BagPanel bagPanel;
    [SerializeField] private StoragePanel storagePanel;
    [SerializeField] private StatusPanel statusPanel;

    [SerializeField] private ActionIcon bagIcon;
    [SerializeField] private ActionIcon storageIcon;
    [SerializeField] private ActionIcon statusIcon;
    [SerializeField] private ActionIcon quitIcon;
    [SerializeField] private StatusText statusText;

    private Dictionary<ReserveActionType, SlidePanel> actionPanels;
    private Dictionary<ReserveActionType, ActionIcon> actionIcons;
    private List<ReserveActionType> actionTypeList;
    private ReserveActionType currentAction = ReserveActionType.Bag;

    private void Start()
    {
        actionPanels = new Dictionary<ReserveActionType, SlidePanel>
        {
            {  ReserveActionType.Bag, bagPanel },
            {  ReserveActionType.Storage, storagePanel },
            {  ReserveActionType.Status, statusPanel },
        };

        actionIcons = new Dictionary<ReserveActionType, ActionIcon>
        {
            {  ReserveActionType.Bag, bagIcon },
            {  ReserveActionType.Storage, storageIcon },
            {  ReserveActionType.Status, statusIcon },
            {  ReserveActionType.Quit, quitIcon },
        };

        actionTypeList = new List<ReserveActionType>(actionIcons.Keys);

        ChangeActiveIcon();
        ChangeActionPanel();
    }

    private void OnEnable()
    {
        // 初回はアクティブなアイコンとアクションパネルを更新する
        if (actionIcons == null || actionPanels == null) return;

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
            if (currentAction == ReserveActionType.Quit)
            {
                OnReserveEnd?.Invoke();
            }
        }
    }

    private void ChoiceAction(bool isLeft)
    {
        // actionPanelsから現在のアクションのインデックスを取得
        // 端まで行ったらループする
        int currentIndex = actionTypeList.IndexOf(currentAction);
        if (currentIndex == actionTypeList.Count)
        {
            // Quitのときはスキップする
            currentAction = ReserveActionType.Quit;
            return;
        }
        if (isLeft)
        {
            if (currentIndex > 0)
            {
                ChangeAction(actionTypeList[currentIndex - 1]);
            }
            else
            {
                ChangeAction(actionTypeList[actionTypeList.Count - 1]);
            }
        }
        else
        {
            if (currentIndex < actionTypeList.Count - 1)
            {
                ChangeAction(actionTypeList[currentIndex + 1]);
            }
            else
            {
                ChangeAction(actionTypeList[0]);
            }
        }
    }

    public void BagPanelOpen()
    {
        ChangeAction(ReserveActionType.Bag);
    }

    public void StoragePanelOpen()
    {
        ChangeAction(ReserveActionType.Storage);
    }

    public void StatusPanelOpen()
    {
        ChangeAction(ReserveActionType.Status);
    }

    public void QuitReserve()
    {
        ChangeAction(ReserveActionType.Quit);
    }

    public void ChangeAction(ReserveActionType actionType)
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
        if (currentAction == ReserveActionType.Quit)
        {
            SoundSystem.Instance.PlaySE(SeType.Select);
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

    public void PanelClose(Action onComplete = null)
    {
        foreach (var kvp in actionPanels)
        {
            if (kvp.Key == currentAction)
            {
                kvp.Value.SetActive(false);
            }
        }
        this.SetActive(false);
        onComplete?.Invoke();
    }
}
