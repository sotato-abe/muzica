using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// BattlePanelとIconの表示と切り替えを管理するクラス
public class TradeActionBoard : SlidePanel
{
    public UnityAction OnTradeEnd;

    [SerializeField] private ItemTradePanel itemTradePanel;
    [SerializeField] private CommandTradePanel commandTradePanel;
    [SerializeField] private TalkPanel talkPanel;
    [SerializeField] private ActionIcon itemIcon;
    [SerializeField] private ActionIcon commandIcon;
    [SerializeField] private ActionIcon talkIcon;
    [SerializeField] private ActionIcon quitIcon;

    private Dictionary<TradeActionType, Panel> actionPanels;
    private Dictionary<TradeActionType, ActionIcon> actionIcons;
    private List<TradeActionType> actionTypeList;
    private TradeActionType currentAction = TradeActionType.Item;

    private void Start()
    {
        actionPanels = new Dictionary<TradeActionType, Panel>
        {
            {  TradeActionType.Item, itemTradePanel },
            {  TradeActionType.Command, commandTradePanel },
            {  TradeActionType.Talk, talkPanel },
        };

        actionIcons = new Dictionary<TradeActionType, ActionIcon>
        {
            {  TradeActionType.Item, itemIcon },
            {  TradeActionType.Command, commandIcon },
            {  TradeActionType.Talk, talkIcon },
            {  TradeActionType.Quit, quitIcon },
        };

        actionTypeList = new List<TradeActionType>(actionIcons.Keys);

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
            if (currentAction == TradeActionType.Quit)
            {
                OnTradeEnd?.Invoke(); // 予約終了イベントを呼び出す
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

    public void ItemPanelOpen()
    {
        ChangeAction(TradeActionType.Item);
    }

    public void CommandPanelOpen()
    {
        ChangeAction(TradeActionType.Command);
    }

    public void TalkPanelOpen()
    {
        ChangeAction(TradeActionType.Talk);
    }

    public void QuitTrade()
    {
        OnTradeEnd?.Invoke();
    }

    public void ChangeAction(TradeActionType actionType)
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
        if (currentAction == TradeActionType.Quit)
        {
            return;
        }

        foreach (var kvp in actionPanels)
        {
            if (kvp.Key == currentAction)
            {
                kvp.Value.PanelOpen();
            }
            else
            {
                kvp.Value.ClosePanel();
            }
        }
    }
}
