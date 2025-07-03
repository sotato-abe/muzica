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

    private Dictionary<ReserveActionType, Panel> actionPanels;
    private Dictionary<ReserveActionType, ActionIcon> actionIcons;
    private List<ReserveActionType> actionTypeList;
    private ReserveActionType currentAction = ReserveActionType.Bag;

    private void Start()
    {
        actionPanels = new Dictionary<ReserveActionType, Panel>
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

        actionTypeList = new List<ReserveActionType>(actionPanels.Keys);

        ChangeActiveIcon();
        ChangeActionPanel();
    }

    private void Update()
    {
        if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentAction = ReserveActionType.Bag;
                ChangeActiveIcon();
                ChangeActionPanel();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentAction = ReserveActionType.Storage;
                ChangeActiveIcon();
                ChangeActionPanel();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentAction = ReserveActionType.Status;
                ChangeActiveIcon();
                ChangeActionPanel();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentAction = ReserveActionType.Quit;
                ChangeActiveIcon();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (currentAction == ReserveActionType.Quit)
            {
                OnReserveEnd?.Invoke(); // 予約終了イベントを呼び出す
            }
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
