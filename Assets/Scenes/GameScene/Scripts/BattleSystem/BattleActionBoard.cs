using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// BattlePanelとIconの表示と切り替えを管理するクラス
public class BattleActionBoard : SlidePanel
{
    public UnityAction OnActionEnd;
    public UnityAction OnBattleEnd;
    [SerializeField] private EquipPanel equip1Panel;
    [SerializeField] private EquipPanel equip2Panel;
    [SerializeField] private PocketPanel pocketPanel;
    [SerializeField] private EscapePanel escapePanel;
    [SerializeField] private ActionIcon equip1Icon;
    [SerializeField] private ActionIcon equip2Icon;
    [SerializeField] private ActionIcon pocketIcon;
    [SerializeField] private ActionIcon escapeIcon;

    private Dictionary<BattleActionType, Panel> actionPanels;
    private Dictionary<BattleActionType, ActionIcon> actionIcons;
    private List<BattleActionType> actionTypeList;
    private BattleActionType currentAction = BattleActionType.Equip1;

    private void Start()
    {
        actionPanels = new Dictionary<BattleActionType, Panel>
        {
            {  BattleActionType.Equip1, equip1Panel },
            {  BattleActionType.Equip2, equip2Panel },
            {  BattleActionType.Pocket, pocketPanel },
            {  BattleActionType.Escape, escapePanel },
        };

        actionIcons = new Dictionary<BattleActionType, ActionIcon>
        {
            {  BattleActionType.Equip1, equip1Icon },
            {  BattleActionType.Equip2, equip2Icon },
            {  BattleActionType.Pocket, pocketIcon },
            {  BattleActionType.Escape, escapeIcon },
        };

        actionTypeList = new List<BattleActionType>(actionPanels.Keys);

        equip1Panel.OnActionEnd += ActionEnd;
        equip2Panel.OnActionEnd += ActionEnd;
        escapePanel.OnActionEnd += ActionEnd;
        escapePanel.OnEscape += BattleEnd; // 逃げるイベントを登録

        ChangeActiveIcon();
        ChangeActionPanel();
    }

    private void Update()
    {
        if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ChangeAction(BattleActionType.Equip1);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ChangeAction(BattleActionType.Equip2);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ChangeAction(BattleActionType.Pocket);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ChangeAction(BattleActionType.Escape);
            }
        }
    }

    public void Equip1PanelOpen()
    {
        ChangeAction(BattleActionType.Equip1);
    }

    public void Equip2PanelOpen()
    {
        ChangeAction(BattleActionType.Equip2);
    }

    public void PocketPanelOpen()
    {
        ChangeAction(BattleActionType.Pocket);
    }

    public void EscapePanelOpen()
    {
        ChangeAction(BattleActionType.Escape);
    }

    public void ChangeAction(BattleActionType actionType)
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

    public void ChangeExecuteActionFlg(bool canExecute = true)
    {
        equip1Panel.ChangeExecuteActionFlg(canExecute);
        equip2Panel.ChangeExecuteActionFlg(canExecute);
        pocketPanel.ChangeExecuteActionFlg(canExecute);
        escapePanel.ChangeExecuteActionFlg(canExecute);
    }

    public void RestartReels()
    {
        equip1Panel.RestartReels();
        equip2Panel.RestartReels();
    }

    public void ActionEnd()
    {
        ChangeExecuteActionFlg(false); // アクションを実行不可能にする
        OnActionEnd?.Invoke();
    }

    public void SetEnemyList(List<Character> enemyList)
    {
        // equip1Panel.SetEnemyList(enemyList);
        // equip2Panel.SetEnemyList(enemyList);
        // pocketPanel.SetEnemyList(enemyList);
        escapePanel.SetEnemyList(enemyList);
    }

    public void BattleEnd()
    {
        OnBattleEnd?.Invoke(); // バトル終了イベントを呼び出す
    }
}
