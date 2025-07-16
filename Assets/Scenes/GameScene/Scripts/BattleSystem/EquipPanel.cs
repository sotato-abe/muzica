using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EquipPanel : Panel
{
    [SerializeField] TargetCommandWindow targetCommandWindow;
    [SerializeField] EquipWindow equipWindow;
    [SerializeField] SlotWindow slotWindow;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopReels();
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

    }

    private void StopReels()
    {
        List<Command> activeCommands = new List<Command>();
        activeCommands.AddRange(slotWindow.StopReels());

        foreach (Command command in activeCommands)
        {
            if (command == null) continue;
            Debug.Log($"コマンドを実行: {command.Base.Name}");
        }
    }
}
