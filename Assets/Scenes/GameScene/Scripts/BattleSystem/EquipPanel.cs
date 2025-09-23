using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 装備を使用した攻撃アクションを管理するパネル
/// </summary>
public class EquipPanel : BattleActionPanel
{
    #region Events
    public UnityAction OnActionEnd;
    #endregion

    #region Serialized Fields
    [Header("UI Components")]
    [SerializeField] private TargetCommandWindow targetCommandWindow;
    [SerializeField] private EquipWindow equipWindow;
    [SerializeField] private SlotWindow slotWindow;
    [SerializeField] private EquipmentInfo equipmentInfo;

    [Header("Character Panels")]
    [SerializeField] private CharacterSubPanel playerSubPanel;
    [SerializeField] private CharacterSubPanel enemySubPanel1;
    [SerializeField] private CharacterSubPanel enemySubPanel2;
    [SerializeField] private CharacterSubPanel enemySubPanel3;
    #endregion

    #region Private Fields
    private Equipment currentEquipment;

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
            subPanel.OnLifeOutAction += LifeOutEnemy; // 敵のサブパネルのライフアウトイベントを登録
        }
    }

    private void OnEnable()
    {
        SetEquipment();
        ShowTargeting();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteAttack();
        }
    }
    #endregion

    #region Equipment Management
    /// <summary>
    /// 装備を設定
    /// </summary>
    public void SetEquipment()
    {
        currentEquipment = PlayerController.Instance.GetEquipmentByBodyPart(bodyPartType);
        if (currentEquipment == null)
        {
            ResetEquipment();
            return;
        }
        slotWindow.gameObject.SetActive(true);
        equipWindow.SetEquipment(currentEquipment);
        equipmentInfo.SetInfo(currentEquipment);
        CheckEnergyCost();
    }

    /// <summary>
    /// 装備をリセット
    /// </summary>
    private void ResetEquipment()
    {
        slotWindow.gameObject.SetActive(false);
        equipWindow.ResetSlot();
        currentEquipment = null;
        ClearTarget();
    }

    /// <summary>
    /// エネルギーコストをチェック
    /// </summary>
    private void CheckEnergyCost()
    {
        if (currentEquipment == null)
        {
            Debug.LogWarning("現在の装備が設定されていません。");
            return;
        }

        bool canUse = PlayerController.Instance.CheckEnergyCost(currentEquipment.EquipmentBase.EnergyCostList);
        equipWindow.SetStatusImage(canUse);
    }

    /// <summary>
    /// 装備の有効性をチェック
    /// </summary>
    private bool IsEquipmentValid()
    {
        if (currentEquipment == null)
        {
            Debug.LogWarning("現在の装備が設定されていません。");
            return false;
        }
        return true;
    }
    #endregion

    #region Target Management
    public void SetTargetting()
    {
        targetSubPanels.Clear();
        if (currentEquipment == null)
        {
            ClearTarget();
            return;
        }

        switch (currentEquipment.EquipmentBase.TargetType)
        {
            case TargetType.Individual:
                SetTargetEnemyPanel(1);
                break;
            case TargetType.Group:
                SetTargetEnemyPanel(3);
                break;
            case TargetType.All:
                SetTargetEnemyPanel(3);
                targetSubPanels.Add(playerSubPanel);
                break;
            case TargetType.Self:
                SetTargetEnemyPanel(0);
                targetSubPanels.Add(playerSubPanel);
                break;
            default:
                Debug.LogWarning("Unknown target type.");
                break;
        }

        foreach (var subPanel in targetSubPanels)
        {
            subPanel.SetTarget(true);
        }
    }
    private void SetTargetEnemyPanel(int index)
    {
        targetSubPanels.Clear();
        foreach (var subPanel in enemySubPanels)
        {
            if (subPanel.isOpen && targetSubPanels.Count < index)
            {
                targetSubPanels.Add(subPanel);
                subPanel.SetTarget(true);
            }
            else
            {
                subPanel.SetTarget(false); // ターゲット以外は非表示にする
            }
        }
    }
    public void ChangeTargetEnemy(CharacterSubPanel enemySubPanel)
    {
        if (currentEquipment == null)
            return;

        targetType = currentEquipment.EquipmentBase.TargetType;
        if (targetType == TargetType.Individual && enemySubPanel.isOpen)
        {
            foreach (var subPanel in targetSubPanels)
            {
                if (subPanel != enemySubPanel)
                {
                    subPanel.SetTarget(false);
                }
            }
            targetSubPanels.Clear();
            targetSubPanels.Add(enemySubPanel);
            enemySubPanel.SetTarget(true);
        }
    }

    public void ShowTargeting()
    {
        if (0 == targetSubPanels.Count)
        {
            SetEquipment();
            SetTargetting();
            return;
        }

        foreach (var subPanel in enemySubPanels)
        {
            if (targetSubPanels.Contains(subPanel))
            {
                if (subPanel.isOpen)
                {
                    subPanel.SetTarget(true);
                }
                else
                {
                    // ターゲットが倒れているときはターゲットを再選択する
                    SetTargetting();
                    break;
                }
            }
            else
            {
                subPanel.SetTarget(false);
            }
        }
    }

    private void ClearTarget()
    {
        foreach (var subPanel in targetSubPanels)
        {
            subPanel.SetTarget(false);
        }
        targetSubPanels.Clear();
    }
    #endregion

    #region Attack Execution
    /// <summary>
    /// 攻撃を実行
    /// </summary>
    public void ExecuteAttack()
    {
        if (!canExecuteActionFlg) return;
        if (!IsEquipmentValid()) return;
        if (!TryUseEnergy()) return;

        canExecuteActionFlg = false;
        StartCoroutine(StopSlot());
    }

    /// <summary>
    /// エネルギーの使用を試行
    /// </summary>
    private bool TryUseEnergy()
    {
        bool isUsed = PlayerController.Instance.UseEnergyCost(currentEquipment.EquipmentBase.EnergyCostList);
        CheckEnergyCost();

        if (!isUsed)
        {
            Debug.LogWarning("エネルギーが不足しています。");
            return false;
        }

        return true;
    }

    /// <summary>
    /// リールを停止
    /// </summary>
    public IEnumerator StopSlot()
    {
        yield return StartCoroutine(slotWindow.StopSlot());
        TotalAttackCount totalCount = equipmentInfo.GetTotalCount();
        if (currentEquipment.EquipmentBase.TargetType == TargetType.Guard)
        {
            ExecuteGuardAction(totalCount);
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            ExecuteTargetAttack(totalCount);
            yield return new WaitForSeconds(0.5f);
        }
    }

    /// <summary>
    /// 攻撃対象を取得
    /// </summary>
    private List<CharacterSubPanel> GetActiveEnemyPanels()
    {
        List<CharacterSubPanel> activePanels = new List<CharacterSubPanel>();

        if (enemySubPanel1.isOpen) activePanels.Add(enemySubPanel1);
        if (enemySubPanel2.isOpen) activePanels.Add(enemySubPanel2);
        if (enemySubPanel3.isOpen) activePanels.Add(enemySubPanel3);

        return activePanels;
    }

    /// <summary>
    /// ガードアクションを実行
    /// </summary>
    private void ExecuteGuardAction(TotalAttackCount totalCount)
    {

        PlayerController.Instance.TakeGuard(totalCount);
        OnActionEnd?.Invoke();
    }

    private void ExecuteTargetAttack(TotalAttackCount totalCount)
    {
        List<CharacterSubPanel> targetSubPanels = new List<CharacterSubPanel>();

        if (enemySubPanel1.isOpen && enemySubPanel1.isTarget) targetSubPanels.Add(enemySubPanel1);
        if (enemySubPanel2.isOpen && enemySubPanel2.isTarget) targetSubPanels.Add(enemySubPanel2);
        if (enemySubPanel3.isOpen && enemySubPanel3.isTarget) targetSubPanels.Add(enemySubPanel3);

        foreach (var subPanel in targetSubPanels)
        {
            StartCoroutine(ExecuteAction(subPanel, totalCount));
        }
    }

    /// <summary>
    /// アクションを実行
    /// </summary>
    private IEnumerator ExecuteAction(CharacterSubPanel characterSubPanel, TotalAttackCount totalCount)
    {
        yield return StartCoroutine(characterSubPanel.TakeAttackCoroutine(totalCount));
        OnActionEnd?.Invoke();
        RestartSlot();
    }

    /// <summary>
    /// リールを再開
    /// </summary>
    public void RestartSlot()
    {
        if (!IsEquipmentValid() || !gameObject.activeInHierarchy)
        {
            return;
        }

        equipmentInfo.SetInfo(currentEquipment);
        StartCoroutine(slotWindow.StartSlot());
    }
    #endregion

    public void LifeOutEnemy(CharacterSubPanel enemySubPanel)
    {
        SetTargetting();
    }
}
