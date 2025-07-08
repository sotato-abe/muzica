using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EquipmentWindow : MonoBehaviour
{
    public UnityAction OnUpdateInventory;
    private const int EQUIPMENT_COUNT = 2;
    [SerializeField] EquipmentSlot equipmentSlot;
    [SerializeField] GameObject equipmentList;
    PlayerController playerController;
    List<EquipmentSlot> equipmentSlots = new List<EquipmentSlot>();

    private void Awake()
    {
        playerController = PlayerController.Instance;
        DeleteAllSlot();
        CreateEquipmentSlot();
        SetEquipments();
    }

    private void DeleteAllSlot()
    {
        foreach (Transform child in equipmentList.transform)
        {
            Destroy(child.gameObject);
        }
        equipmentSlots.Clear();
    }

    public void CreateEquipmentSlot()
    {
        if (PlayerController.Instance == null) return;

        PlayerController playerController = PlayerController.Instance;
        if (playerController.PlayerCharacter == null) return;

        for (int i = 0; i < EQUIPMENT_COUNT; i++)
        {
            EquipmentSlot slot = Instantiate(equipmentSlot, equipmentList.transform);
            slot.OnUpdateInventory += UpdateInventory;
            equipmentSlots.Add(slot);
        }
    }

    public void SetEquipments()
    {
        List<Equipment> equipmentList = PlayerController.Instance.PlayerCharacter.EquipmentList;
        if (equipmentList == null || equipmentList.Count == 0)
        {
            return;
        }
        if (equipmentList.Count > equipmentSlots.Count)
        {
            return;
        }
        for (int i = 0; i < equipmentList.Count; i++)
        {
            if (i < equipmentSlots.Count)
            {
                EquipmentSlot slot = equipmentSlots[i];
                Equipment equip = equipmentList[i];
                EquipmentSlot slotComponent = slot.GetComponent<EquipmentSlot>();
                slotComponent.SetEquipment(equip);
            }
            else
            {
                Debug.LogWarning($"装備スロットの数が不足しています。装備リストのアイテム数: {equipmentList.Count}, 装備スロット数: {equipmentSlots.Count}");
                break;
            }
        }
    }

    public void UpdateInventory()
    {
        OnUpdateInventory?.Invoke();
    }
}
