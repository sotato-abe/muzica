using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 装備を使用した攻撃アクションを管理するパネル
/// </summary>
public class EquipPanel : BattleActionPanel
{
    #region Events
    public UnityAction OnActionStart;
    public UnityAction OnActionEnd;
    #endregion

    #region Serialized Fields
    [Header("UI Components")]
    [SerializeField] private TargetCommandWindow targetCommandWindow;
    [SerializeField] private EquipWindow equipWindow;
    [SerializeField] private SlotWindow slotWindow;
    [SerializeField] private EquipmentInfo equipmentInfo;
    [SerializeField] private Image activeButtonImage;

    [Header("Character Panels")]
    [SerializeField] private CharacterSubPanel playerSubPanel;
    [SerializeField] private CharacterSubPanel enemySubPanel1;
    [SerializeField] private CharacterSubPanel enemySubPanel2;
    [SerializeField] private CharacterSubPanel enemySubPanel3;
    #endregion

    #region Private Fields
    private Equipment currentEquipment;
    private Color defaultButtonColor = new Color(130f / 255f, 130f / 255f, 130f / 255f, 255f / 255f);
    private Color activeButtonColor = new Color(240f / 255f, 88f / 255f, 0f / 255f, 255f / 255f);

    List<CharacterSubPanel> enemySubPanels = new List<CharacterSubPanel>(); // 敵のサブパネルリスト
    List<CharacterSubPanel> targetSubPanels = new List<CharacterSubPanel>();

    [Header("Equipment Settings")]
    public BodyPartType bodyPartType = BodyPartType.None;
    public int equipmentNum = 0;
    private List<EnergyCount> energyAttackList;
    private TargetType targetType;
    private List<EnergyCost> energyCostList;
    private List<Enchant> enchantList;
    #endregion

    #region Unity Lifecycle
    protected override void Awake()
    {
        base.Awake();
        enemySubPanels.Add(enemySubPanel3);
        enemySubPanels.Add(enemySubPanel2);
        enemySubPanels.Add(enemySubPanel1);

        foreach (var subPanel in enemySubPanels)
        {
            subPanel.OnTarget += ChangeTargetEnemy;
            subPanel.OnLifeOutAction += LifeOutEnemy;
        }
    }

    private void OnEnable()
    {
        SetEquipment();
        ShowTargetting();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteEquipAction();
        }
    }
    #endregion

    #region Equipment Management
    public void SetEquipment()
    {
        currentEquipment = PlayerController.Instance.GetEquipmentByBodyPart(bodyPartType);
        if (currentEquipment == null)
        {
            slotWindow.gameObject.SetActive(false);
            equipWindow.ResetSlot();
            currentEquipment = null;
            ClearTarget();
            return;
        }
        slotWindow.gameObject.SetActive(true);
        equipWindow.SetEquipment(currentEquipment);
        equipmentInfo.SetInfo(currentEquipment);
        ChangeEquipStatus();
    }

    private void ChangeEquipStatus()
    {
        if (!IsEquipmentValid())
        {
            activeButtonImage.color = defaultButtonColor;
            return;
        }
        bool canUse = PlayerController.Instance.CheckEnergyCost(currentEquipment.EquipmentBase.EnergyCostList);
        equipWindow.SetStatusImage(canUse);
        if (canUse && canExecuteActionFlg)
            activeButtonImage.color = activeButtonColor;
        else
            activeButtonImage.color = defaultButtonColor;
    }
    #endregion

    #region Target Management
    private void ShowTargetting()
    {
        if (targetSubPanels.Count == 0)
        {
            SetTargetting(); // ターゲットが設定されていない場合は新たに設定
            return;
        }

        foreach (var subPanel in enemySubPanels)
        {
            if (targetSubPanels.Contains(subPanel))
            {
                if (!subPanel.isOpen)
                {
                    SetTargetting(); // ターゲットに含まれているけど閉じている場合はターゲットを再設定
                    return;
                }
                else
                {
                    subPanel.SetTarget(true); // ターゲットに含まれていて開いている場合はターゲット表示を維持
                    continue;
                }
            }
            if (subPanel.isTarget)
            {
                subPanel.SetTarget(false); // ターゲットに含まれていない場合はターゲット表示を解除
                continue;
            }
        }
    }

    public void HideTarget()
    {
        foreach (var subPanel in enemySubPanels)
        {
            subPanel.SetTarget(false);
        }
        playerSubPanel.SetTarget(false);
    }

    private void ClearTarget()
    {
        HideTarget();
        targetSubPanels.Clear();
    }

    public void SetTargetting()
    {
        ClearTarget();
        if (!IsEquipmentValid())
            return;
        if (currentEquipment.EquipmentBase.EquipmentType == EquipmentType.Armor)
        {
            playerSubPanel.SetTarget(true);
            return;
        }

        switch (currentEquipment.EquipmentBase.TargetType)
        {
            case TargetType.Individual:
                playerSubPanel.SetTarget(false);
                SetTargetEnemyPanel(1);
                break;
            case TargetType.Group:
                playerSubPanel.SetTarget(false);
                SetTargetEnemyPanel(3);
                break;
            case TargetType.All:
                playerSubPanel.SetTarget(true);
                SetTargetEnemyPanel(3);
                targetSubPanels.Add(playerSubPanel);
                break;
            default:
                break;
        }
    }

    private void SetTargetEnemyPanel(int index)
    {
        foreach (var subPanel in enemySubPanels)
        {
            if (subPanel.isOpen && targetSubPanels.Count < index)
            {
                targetSubPanels.Add(subPanel);
                subPanel.SetTarget(true);
            }
            else
            {
                subPanel.SetTarget(false);
            }
        }
    }

    public void ChangeTargetEnemy(CharacterSubPanel enemySubPanel)
    {
        if (currentEquipment == null)
            return;
        if (currentEquipment.EquipmentBase.EquipmentType == EquipmentType.Armor)
            return;

        if (currentEquipment.EquipmentBase.TargetType == TargetType.Individual && enemySubPanel.isOpen)
        {
            ClearTarget();
            targetSubPanels.Add(enemySubPanel);
            enemySubPanel.SetTarget(true);
        }
    }
    #endregion

    #region EquipAction Execution
    public override void ChangeExecuteActionFlg(bool canExecute)
    {
        base.ChangeExecuteActionFlg(canExecute);
        if(currentEquipment == null)
        {
            activeButtonImage.color = defaultButtonColor;
            return;
        }
        bool canUse = PlayerController.Instance.CheckEnergyCost(currentEquipment.EquipmentBase.EnergyCostList);
        if (canExecute && canUse)
            activeButtonImage.color = activeButtonColor;
        else
            activeButtonImage.color = defaultButtonColor;
    }

    public void ExecuteEquipAction()
    {
        if (!canExecuteActionFlg) return;
        if (!IsEquipmentValid()) return;
        if (!TryUseEnergy()) return;

        OnActionStart?.Invoke();
        canExecuteActionFlg = false;
        StartCoroutine(StopSlot());
    }

    private bool TryUseEnergy()
    {
        bool isUsed = PlayerController.Instance.UseEnergyCost(currentEquipment.EquipmentBase.EnergyCostList);
        equipWindow.SetStatusImage(isUsed);
        return isUsed;
    }

    public IEnumerator StopSlot()
    {
        yield return StartCoroutine(slotWindow.StopSlot());
        TotalAttackCount totalCount = equipmentInfo.GetTotalCount();
        if (currentEquipment.EquipmentBase.EquipmentType == EquipmentType.Armor)
        {
            ExecuteGuardAction(totalCount);
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            ExecuteAttackAction(totalCount);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void ExecuteGuardAction(TotalAttackCount totalCount)
    {
        PlayerController.Instance.TakeGuard(totalCount);
        OnActionEnd?.Invoke();
    }

    private void ExecuteAttackAction(TotalAttackCount totalCount)
    {
        List<CharacterSubPanel> targetSubPanels = new List<CharacterSubPanel>();

        if (enemySubPanel1.isOpen && enemySubPanel1.isTarget) targetSubPanels.Add(enemySubPanel1);
        if (enemySubPanel2.isOpen && enemySubPanel2.isTarget) targetSubPanels.Add(enemySubPanel2);
        if (enemySubPanel3.isOpen && enemySubPanel3.isTarget) targetSubPanels.Add(enemySubPanel3);

        foreach (var subPanel in targetSubPanels)
        {
            StartCoroutine(ExecuteAttack(subPanel, totalCount));
        }
    }

    private IEnumerator ExecuteAttack(CharacterSubPanel characterSubPanel, TotalAttackCount totalCount)
    {
        yield return StartCoroutine(characterSubPanel.TakeAttackCoroutine(totalCount));
        OnActionEnd?.Invoke();
        RestartSlot();
    }

    public void RestartSlot()
    {
        if (!IsEquipmentValid() || !gameObject.activeInHierarchy)
            return;
        equipmentInfo.SetInfo(currentEquipment);
        StartCoroutine(slotWindow.StartSlot());
    }
    #endregion

    private bool IsEquipmentValid()
    {
        if (currentEquipment == null)
        {
            UnityEngine.Debug.LogWarning("現在の装備が設定されていません。");
            return false;
        }
        return true;
    }

    public void LifeOutEnemy(CharacterSubPanel enemySubPanel)
    {
        // gameObjectが非アクティブの場合は処理を中断
        if (!this.gameObject.activeInHierarchy) return;
        // targetSubPanelsからライフアウトした敵のサブパネルを削除
        UnityEngine.Debug.Log($"Enemy Life Out Detected : {currentEquipment.EquipmentBase?.Name}");
        targetSubPanels.Remove(enemySubPanel);
        // SetEquipment();
        SetTargetting();
    }
}
