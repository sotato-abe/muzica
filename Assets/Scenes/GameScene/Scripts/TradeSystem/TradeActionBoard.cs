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

        ChangeActiveIcon();
        ChangeActionPanel();
    }

    private void Update()
    {
        if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ChangeAction(TradeActionType.Item);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ChangeAction(TradeActionType.Command);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ChangeAction(TradeActionType.Talk);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ChangeAction(TradeActionType.Quit);
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
        if (actionType == TradeActionType.Quit)
        {
            OnTradeEnd?.Invoke(); // 予約終了イベントを呼び出す
            return;
        }
        else if (actionPanels.ContainsKey(actionType))
        {
            currentAction = actionType;
            ChangeActiveIcon();
            ChangeActionPanel();
        }
        else
        {
            Debug.LogWarning($"Action type {actionType} is not defined in action panels.");
        }
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
