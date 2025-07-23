using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EquipPanel : BattleActionPanel
{
    public UnityAction OnActionEnd;

    [SerializeField] TargetCommandWindow targetCommandWindow;
    [SerializeField] EquipWindow equipWindow;
    [SerializeField] SlotWindow slotWindow;
    [SerializeField] EquipmentInfo equipmentInfo;

    [SerializeField] private CharacterSubPanel playerSubPanel; // キャラクターサブパネル
    [SerializeField] private CharacterSubPanel enemySubPanel1; // キャラクターサブパネル
    [SerializeField] private CharacterSubPanel enemySubPanel2; // キャラクターサブパネル
    [SerializeField] private CharacterSubPanel enemySubPanel3; // キャラクターサブパネル

    PlayerController playerController;
    public int equipmentNum = 0;
    private Equipment currentEquipment;

    private List<EnergyCount> energyAttackList;
    private TargetType TargetType;
    private List<EnergyCost> EnergyCostList;
    private List<Enchant> EnchantList;

    private void Start()
    {
        playerController = PlayerController.Instance;
    }
    private void OnEnable()
    {
        if (PlayerController.Instance == null) return;

        playerController = PlayerController.Instance;

        int equipmentCount = playerController.PlayerCharacter.EquipmentList.Count;
        Equipment equipment = null;
        if (equipmentNum < equipmentCount)
        {
            equipment = playerController.PlayerCharacter.EquipmentList[equipmentNum];
        }
        SetEquipment(equipment);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteAttack();
        }
    }

    public void SetEquipment(Equipment equipment)
    {
        if (equipment == null)
        {
            equipWindow.ResetSlot();
            currentEquipment = null;
            return;
        }
        equipWindow.SetEquipment(equipment);
        currentEquipment = equipment;
        CheckEnegryCost();
    }

    private void CheckEnegryCost()
    {
        if (currentEquipment == null)
        {
            Debug.LogWarning("現在の装備が設定されていません。");
            return;
        }
        bool canUse = playerController.CheckEnergyCost(currentEquipment.EquipmentBase.EnergyCostList);
        equipWindow.SetStatusImage(canUse);
    }

    public void TargetCommand(Command Command)
    {
        targetCommandWindow.TargetCommand(Command);
    }

    public void ExecuteAttack()
    {
        if (currentEquipment == null)
        {
            Debug.LogWarning("現在の装備が設定されていません。");
            return;
        }
        bool isUsed = playerController.UseEnergyCost(currentEquipment.EquipmentBase.EnergyCostList);
        CheckEnegryCost();
        if (!isUsed)
        {
            Debug.LogWarning("エネルギーが不足しています。");
            return;
        }
        StopReels();
    }

    private void StopReels()
    {
        List<Command> activeCommands = new List<Command>();
        slotWindow.StopReels(result =>
        {
            foreach (var cmd in result)
            {
                if (cmd == null)
                {
                    continue;
                }

                activeCommands.Add(cmd);
            }
            ActionStart(activeCommands);

        });
    }

    private void ActionStart(List<Command> commands = null)
    {
        if (commands.Count != 0)
        {
            foreach (var command in commands)
            {
                if (command == null || command.Base == null)
                {
                    Debug.LogWarning("コマンドのBaseが未設定です。");
                    continue;
                }
                equipmentInfo.CommandUpdate(command);
            }
        }
        AttackVector();
    }

    private void AttackVector()
    {
        // activeなパネルを取得
        TotalAttackCount totalCount = equipmentInfo.GetTotalCount();
        List<CharacterSubPanel> activePanels = new List<CharacterSubPanel>();
        if (enemySubPanel1.isActive)
        {
            activePanels.Add(enemySubPanel1);
        }
        if (enemySubPanel2.isActive)
        {
            activePanels.Add(enemySubPanel2);
        }
        if (enemySubPanel3.isActive)
        {
            activePanels.Add(enemySubPanel3);
        }


        // return activePanels;
        switch (totalCount.TargetType)
        {
            case TargetType.Self:
                // 自分自身への攻撃
                if (playerSubPanel.isActive)
                {
                    StartCoroutine(ExecuteAction(playerSubPanel, totalCount));
                }
                else
                {
                    Debug.LogWarning("プレイヤーのキャラクターサブパネルがアクティブではありません。");
                }
                break;
            case TargetType.Individual:
                // 個別攻撃の処理
                if (activePanels.Count > 0)
                {
                    CharacterSubPanel targetPanel = activePanels[activePanels.Count - 1];
                    StartCoroutine(ExecuteAction(targetPanel, totalCount));
                }
                else
                {
                    Debug.LogWarning("攻撃対象のキャラクターサブパネルがありません。");
                }
                break;
            case TargetType.Group:
                // 一方攻撃の処理 
                foreach (var subPanel in activePanels)
                {
                    StartCoroutine(ExecuteAction(subPanel, totalCount));
                }
                break;
            case TargetType.All:
                // 全体攻撃の処理
                foreach (var subPanel in activePanels)
                {
                    StartCoroutine(ExecuteAction(subPanel, totalCount));
                }
                StartCoroutine(ExecuteAction(playerSubPanel, totalCount));
                break;
            case TargetType.Random:
                // ランダム攻撃の処理
                if (activePanels.Count > 0)
                {
                    CharacterSubPanel targetPanel = activePanels[Random.Range(0, activePanels.Count)];
                    StartCoroutine(ExecuteAction(targetPanel, totalCount));
                }
                break;
            default:
                Debug.LogWarning("不明なターゲットタイプです。");
                break;
        }
    }

    private IEnumerator ExecuteAction(CharacterSubPanel characterSubPanel, TotalAttackCount totalCount)
    {
        yield return StartCoroutine(characterSubPanel.TakeAttack(totalCount)); // キャラクターサブパネルに攻撃を実行
        OnActionEnd?.Invoke();
        RestartReels();
    }

    public void RestartReels()
    {
        equipmentInfo.SetInfo(currentEquipment); // 装備情報を更新

        if (currentEquipment == null || !gameObject.activeInHierarchy)
        {
            Debug.LogWarning("現在の装備が設定されていません。");
            return;
        }
        StartCoroutine(slotWindow.StartReels());
    }

    public override void ChangeExecuteActionFlg(bool canExecute)
    {
        if (currentEquipment == null)
        {
            Debug.LogWarning("現在の装備が設定されていません。");
            return;
        }
        base.ChangeExecuteActionFlg(canExecute);
    }
}
