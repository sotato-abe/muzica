using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EquipPanel : Panel
{
    [SerializeField] TargetCommandWindow targetCommandWindow;
    [SerializeField] EquipWindow equipWindow;
    [SerializeField] SlotPanel slotPanel;
    PlayerController playerController;
    public int equipmentNum = 0;

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

    public void SetEquipment(Equipment equipment)
    {
        if (equipment == null)
        {
            equipWindow.ResetSlot();
            return;
        }
        equipWindow.SetEquipment(equipment);
    }

    public void ResetEquipment()
    {
        equipWindow.ResetSlot();
    }

    public void TargetCommand(Command Command)
    {
        targetCommandWindow.TargetCommand(Command);
    }
}
