using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// BattlePanelとIconの表示と切り替えを管理するクラス
public class BattleActionBoard : SlidePanel
{
    public UnityAction OnActionEnd;
    public UnityAction OnBattleEnd;
    [SerializeField] private EquipPanel equipPanel1;
    [SerializeField] private EquipPanel equipPanel2;
    [SerializeField] private PocketPanel pocketPanel;
    [SerializeField] private EscapePanel escapePanel;
    [SerializeField] private ActionIcon equipIcon1;
    [SerializeField] private ActionIcon equipIcon2;
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
            {  BattleActionType.Equip1, equipPanel1 },
            {  BattleActionType.Equip2, equipPanel2 },
            {  BattleActionType.Pocket, pocketPanel },
            {  BattleActionType.Escape, escapePanel },
        };

        actionIcons = new Dictionary<BattleActionType, ActionIcon>
        {
            {  BattleActionType.Equip1, equipIcon1 },
            {  BattleActionType.Equip2, equipIcon2 },
            {  BattleActionType.Pocket, pocketIcon },
            {  BattleActionType.Escape, escapeIcon },
        };

        actionTypeList = new List<BattleActionType>(actionPanels.Keys);

        equipPanel1.OnActionEnd += ActionEnd;
        equipPanel2.OnActionEnd += ActionEnd;
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

    public void equipPanel1Open()
    {
        ChangeAction(BattleActionType.Equip1);
    }

    public void equipPanel2Open()
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
        equipPanel1.ChangeExecuteActionFlg(canExecute);
        equipPanel2.ChangeExecuteActionFlg(canExecute);
        pocketPanel.ChangeExecuteActionFlg(canExecute);
        escapePanel.ChangeExecuteActionFlg(canExecute);
    }

    public void ActionEnd()
    {
        ChangeExecuteActionFlg(false); // アクションを実行不可能にする
        OnActionEnd?.Invoke();
    }

    public void SetEnemyList(List<Character> enemyList)
    {
        // equipPanel1.SetEnemyList(enemyList);
        // equipPanel2.SetEnemyList(enemyList);
        // pocketPanel.SetEnemyList(enemyList);
        escapePanel.SetEnemyList(enemyList);
    }

    public void BattleEnd()
    {
        OnBattleEnd?.Invoke(); // バトル終了イベントを呼び出す
    }
}
