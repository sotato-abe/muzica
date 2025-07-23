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
    private PlayerController playerController;
    private Equipment currentEquipment;
    
    [Header("Equipment Settings")]
    public int equipmentNum = 0;

    // 未使用フィールド（将来の拡張用）
    private List<EnergyCount> energyAttackList;
    private TargetType targetType;
    private List<EnergyCost> energyCostList;
    private List<Enchant> enchantList;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        InitializeController();
    }

    private void OnEnable()
    {
        InitializeEquipment();
    }

    private void Update()
    {
        HandleInput();
    }
    #endregion

    #region Initialization
    private void InitializeController()
    {
        playerController = PlayerController.Instance;
    }

    private void InitializeEquipment()
    {
        if (PlayerController.Instance == null) return;

        playerController = PlayerController.Instance;
        Equipment equipment = GetCurrentEquipment();
        SetEquipment(equipment);
    }

    private Equipment GetCurrentEquipment()
    {
        int equipmentCount = playerController.PlayerCharacter.EquipmentList.Count;
        
        if (equipmentNum < equipmentCount)
        {
            return playerController.PlayerCharacter.EquipmentList[equipmentNum];
        }
        
        return null;
    }
    #endregion

    #region Input Handling
    private void HandleInput()
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
    public void SetEquipment(Equipment equipment)
    {
        if (equipment == null)
        {
            ResetEquipment();
            return;
        }

        equipWindow.SetEquipment(equipment);
        currentEquipment = equipment;
        CheckEnergyCost();
    }

    /// <summary>
    /// 装備をリセット
    /// </summary>
    private void ResetEquipment()
    {
        equipWindow.ResetSlot();
        currentEquipment = null;
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

        bool canUse = playerController.CheckEnergyCost(currentEquipment.EquipmentBase.EnergyCostList);
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

    #region Command Targeting
    /// <summary>
    /// コマンドをターゲット
    /// </summary>
    public void TargetCommand(Command command)
    {
        targetCommandWindow.TargetCommand(command);
    }
    #endregion

    #region Attack Execution
    /// <summary>
    /// 攻撃を実行
    /// </summary>
    public void ExecuteAttack()
    {
        if (!IsEquipmentValid()) return;

        if (!TryUseEnergy()) return;

        StopReels();
    }

    /// <summary>
    /// エネルギーの使用を試行
    /// </summary>
    private bool TryUseEnergy()
    {
        bool isUsed = playerController.UseEnergyCost(currentEquipment.EquipmentBase.EnergyCostList);
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
    private void StopReels()
    {
        slotWindow.StopReels(result =>
        {
            List<Command> activeCommands = FilterValidCommands(result);
            ActionStart(activeCommands);
        });
    }

    /// <summary>
    /// 有効なコマンドをフィルタリング
    /// </summary>
    private List<Command> FilterValidCommands(List<Command> commands)
    {
        List<Command> activeCommands = new List<Command>();

        foreach (var cmd in commands)
        {
            if (cmd != null)
            {
                activeCommands.Add(cmd);
            }
        }

        return activeCommands;
    }

    /// <summary>
    /// アクションを開始
    /// </summary>
    private void ActionStart(List<Command> commands)
    {
        if (commands.Count > 0)
        {
            UpdateEquipmentInfo(commands);
        }

        ExecuteAttackVector();
    }

    /// <summary>
    /// 装備情報を更新
    /// </summary>
    private void UpdateEquipmentInfo(List<Command> commands)
    {
        foreach (var command in commands)
        {
            if (command?.Base == null)
            {
                Debug.LogWarning("コマンドのBaseが未設定です。");
                continue;
            }

            equipmentInfo.CommandUpdate(command);
        }
    }
    #endregion

    #region Target Management
    /// <summary>
    /// 攻撃対象を取得
    /// </summary>
    private List<CharacterSubPanel> GetActiveEnemyPanels()
    {
        List<CharacterSubPanel> activePanels = new List<CharacterSubPanel>();

        if (enemySubPanel1.isActive) activePanels.Add(enemySubPanel1);
        if (enemySubPanel2.isActive) activePanels.Add(enemySubPanel2);
        if (enemySubPanel3.isActive) activePanels.Add(enemySubPanel3);

        return activePanels;
    }

    /// <summary>
    /// 攻撃ベクトルを実行
    /// </summary>
    private void ExecuteAttackVector()
    {
        TotalAttackCount totalCount = equipmentInfo.GetTotalCount();
        List<CharacterSubPanel> activePanels = GetActiveEnemyPanels();

        switch (totalCount.TargetType)
        {
            case TargetType.Self:
                ExecuteSelfAttack(totalCount);
                break;
            case TargetType.Individual:
                ExecuteIndividualAttack(activePanels, totalCount);
                break;
            case TargetType.Group:
                ExecuteGroupAttack(activePanels, totalCount);
                break;
            case TargetType.All:
                ExecuteAllAttack(activePanels, totalCount);
                break;
            case TargetType.Random:
                ExecuteRandomAttack(activePanels, totalCount);
                break;
            default:
                Debug.LogWarning("不明なターゲットタイプです。");
                break;
        }
    }

    /// <summary>
    /// 自分自身への攻撃
    /// </summary>
    private void ExecuteSelfAttack(TotalAttackCount totalCount)
    {
        if (playerSubPanel.isActive)
        {
            StartCoroutine(ExecuteAction(playerSubPanel, totalCount));
        }
        else
        {
            Debug.LogWarning("プレイヤーのキャラクターサブパネルがアクティブではありません。");
        }
    }

    /// <summary>
    /// 個別攻撃
    /// </summary>
    private void ExecuteIndividualAttack(List<CharacterSubPanel> activePanels, TotalAttackCount totalCount)
    {
        if (activePanels.Count > 0)
        {
            CharacterSubPanel targetPanel = activePanels[activePanels.Count - 1];
            StartCoroutine(ExecuteAction(targetPanel, totalCount));
        }
        else
        {
            Debug.LogWarning("攻撃対象のキャラクターサブパネルがありません。");
        }
    }

    /// <summary>
    /// グループ攻撃
    /// </summary>
    private void ExecuteGroupAttack(List<CharacterSubPanel> activePanels, TotalAttackCount totalCount)
    {
        foreach (var subPanel in activePanels)
        {
            StartCoroutine(ExecuteAction(subPanel, totalCount));
        }
    }

    /// <summary>
    /// 全体攻撃
    /// </summary>
    private void ExecuteAllAttack(List<CharacterSubPanel> activePanels, TotalAttackCount totalCount)
    {
        foreach (var subPanel in activePanels)
        {
            StartCoroutine(ExecuteAction(subPanel, totalCount));
        }
        StartCoroutine(ExecuteAction(playerSubPanel, totalCount));
    }

    /// <summary>
    /// ランダム攻撃
    /// </summary>
    private void ExecuteRandomAttack(List<CharacterSubPanel> activePanels, TotalAttackCount totalCount)
    {
        if (activePanels.Count > 0)
        {
            CharacterSubPanel targetPanel = activePanels[Random.Range(0, activePanels.Count)];
            StartCoroutine(ExecuteAction(targetPanel, totalCount));
        }
    }
    #endregion

    #region Action Execution
    /// <summary>
    /// アクションを実行
    /// </summary>
    private IEnumerator ExecuteAction(CharacterSubPanel characterSubPanel, TotalAttackCount totalCount)
    {
        yield return StartCoroutine(characterSubPanel.TakeAttackCoroutine(totalCount));
        OnActionEnd?.Invoke();
        RestartReels();
    }

    /// <summary>
    /// リールを再開
    /// </summary>
    public void RestartReels()
    {
        if (!IsEquipmentValid() || !gameObject.activeInHierarchy)
        {
            return;
        }

        equipmentInfo.SetInfo(currentEquipment);
        StartCoroutine(slotWindow.StartReels());
    }
    #endregion

    #region Action Panel Override
    /// <summary>
    /// アクション実行フラグを変更
    /// </summary>
    public override void ChangeExecuteActionFlg(bool canExecute)
    {
        if (!IsEquipmentValid()) return;

        base.ChangeExecuteActionFlg(canExecute);
    }
    #endregion
}
