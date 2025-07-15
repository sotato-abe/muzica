using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EquipPanel : Panel
{
    [SerializeField] EquipmentSlot equipmentSlot;
    [SerializeField] SlotPanel slotPanel;
    PlayerController playerController;
    public int equipmentNum = 0;

    private void Start()
    {
        playerController = PlayerController.Instance;
    }
    private void OnEnable()
    {
        Debug.Log($"EquipPanel OnEnable called with equipmentNum: {equipmentNum}");
        if (PlayerController.Instance == null) return;

        playerController = PlayerController.Instance;

        int equipmentCount = playerController.PlayerCharacter.EquipmentList.Count;
        Equipment equipment = null;
        if (equipmentNum < equipmentCount)
        {
            equipment = playerController.PlayerCharacter.EquipmentList[equipmentNum];
            Debug.Log($"EquipPanel OnEnable: {equipmentNum}: {equipment.Base.Name}");
        }
        SetEquipment(equipment);
    }

    public void SetEquipment(Equipment equipment)
    {
        if (equipment == null)
        {
            equipmentSlot.ResetSlot();
            return;
        }
        equipmentSlot.SetEquipment(equipment);
    }

    public void ResetEquipment()
    {
        equipmentSlot.ResetSlot();
    }
}
