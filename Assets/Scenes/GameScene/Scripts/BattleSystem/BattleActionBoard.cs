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
    [SerializeField] AutoButton autoButton;

    private Dictionary<BattleActionType, Panel> actionPanels;
    private Dictionary<BattleActionType, ActionIcon> actionIcons;
    private List<BattleActionType> actionTypeList;
    private BattleActionType currentAction = BattleActionType.Equip1;
    bool canChangeActionFlg = true;

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
        equipPanel1.OnActionStart += ActionStart;
        equipPanel2.OnActionEnd += ActionEnd;
        equipPanel2.OnActionStart += ActionStart;
        escapePanel.OnActionEnd += ActionEnd;
        escapePanel.OnEscape += BattleEnd; // 逃げるイベントを登録

        ChangeActiveIcon();
        ChangeActionPanel();
    }

    private void Update()
    {
        if (!canChangeActionFlg) return;
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
    }

    public void SetCanChangeActionFlg(bool canChange)
    {
        canChangeActionFlg = canChange;
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
        SoundSystem.Instance.PlaySE(SeType.Select);
        if (currentAction == BattleActionType.Equip1 || currentAction == BattleActionType.Equip2)
        {
            autoButton.gameObject.SetActive(true);
        }
        else
        {
            autoButton.gameObject.SetActive(false);
        }
        foreach (var kvp in actionPanels)
        {
            if (kvp.Key == currentAction)
            {
                kvp.Value.WindowOpen();
                if (kvp.Value is not EquipPanel)
                {
                    equipPanel1.HideTarget();
                }
            }
            else
            {
                kvp.Value.WindowClose();
            }
        }
    }

    public void ChangeExecuteActionFlg(bool canExecute = true)
    {
        equipPanel1.ChangeExecuteActionFlg(canExecute);
        equipPanel2.ChangeExecuteActionFlg(canExecute);
        pocketPanel.ChangeExecuteActionFlg(canExecute);
        escapePanel.ChangeExecuteActionFlg(canExecute);

        // オートバトル
        if (autoButton.IsAuto)
        {
            if (currentAction == BattleActionType.Equip1)
            {
                // 現在のパネルを取得
                equipPanel1.ExecuteEquipAction();
            }
            else if (currentAction == BattleActionType.Equip2)
            {
                // 現在のパネルを取得
                equipPanel2.ExecuteEquipAction();
            }
        }
    }

    public void ActionStart()
    {
        SetCanChangeActionFlg(false); // アクションの切り替えを不可能にする
    }

    public void ActionEnd()
    {
        SetCanChangeActionFlg(true); // アクションの切り替えを可能にする
        ChangeExecuteActionFlg(false); // アクションを実行不可能にする
        OnActionEnd?.Invoke();
    }

    public void SetEnemyList(List<Character> enemyList)
    {
        escapePanel.SetEnemyList(enemyList);
    }

    public void EnemyAppearanced()
    {
        if (equipPanel1.isActive)
        {
            equipPanel1.SetTargetting();
        }
        if (equipPanel2.isActive)
        {
            equipPanel2.SetTargetting();
        }
    }

    public void BattleEnd()
    {
        OnBattleEnd?.Invoke(); // バトル終了イベントを呼び出す
    }
}
