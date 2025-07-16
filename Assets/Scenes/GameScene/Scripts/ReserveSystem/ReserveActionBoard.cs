using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// BattlePanelとIconの表示と切り替えを管理するクラス
public class ReserveActionBoard : MonoBehaviour
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
                ChangeAction(ReserveActionType.Bag);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ChangeAction(ReserveActionType.Storage);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ChangeAction(ReserveActionType.Status);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ChangeAction(ReserveActionType.Quit);
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
        ChangeAction(ReserveActionType.Status);
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
        if (currentAction == ReserveActionType.Quit)
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
