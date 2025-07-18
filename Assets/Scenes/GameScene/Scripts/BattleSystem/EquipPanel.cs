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

        if (!canExecuteActionFlg)
        {
            Debug.LogWarning("アクションが実行できません。");
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
            ExecuteAction(activeCommands);

        });
    }

    private void ExecuteAction(List<Command> commands = null)
    {
        Debug.Log($"EquipPanel：アクションを実行: {currentEquipment?.Base?.Name ?? "未設定"}");
        if (commands.Count != 0)
        {
            // コマンドの効果をequipmentInfoに適用
            // currentEquipmentを使い捨てのコピー
            // Equipment equipment = new Equipment(currentEquipment);


            foreach (var command in commands)
            {
                if (command == null || command.Base == null)
                {
                    Debug.LogWarning("コマンドのBaseが未設定です。");
                    continue;
                }

                // コマンドの効果を適用する処理
                // ここではコマンドの効果を適用するためのメソッドを呼び出す
                equipmentInfo.CommandUpdate(command);
            }

            // Infoの数値を実行

        }
        // 結果を使って次の処理へ
        StartCoroutine(ActionEnd());
    }

    private IEnumerator ActionEnd()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("EquipPanel：アクションが終了しました");
        OnActionEnd?.Invoke();
        RestartReels();
    }

    public void RestartReels()
    {
        Debug.Log($"EquipPanel：リールを再起動: {currentEquipment?.Base?.Name ?? "未設定"}");
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
