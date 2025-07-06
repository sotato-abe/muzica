using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EquipmentWindow : MonoBehaviour
{
    [SerializeField] EquipmentSlot equipmentSlot;
    [SerializeField] GameObject equipmentList;
    PlayerController playerController;
    List<EquipmentSlot> equipmentSlots = new List<EquipmentSlot>();

    int equipmentCount = 1;

    private void Awake()
    {
        playerController = PlayerController.Instance;
        DeleteAllSlot();
        CreateEquipmentSlot();
    }
    private void OnEnable()
    {
        if (PlayerController.Instance == null) return;

        playerController = PlayerController.Instance;
        SetEquipments();
    }

    private void DeleteAllSlot()
    {
        if (equipmentSlots.Count == 0) return;

        foreach (EquipmentSlot slot in equipmentSlots)
        {
            Destroy(slot.gameObject);
        }
        equipmentSlots.Clear();
    }

    public void CreateEquipmentSlot()
    {
        if (PlayerController.Instance == null) return;

        PlayerController playerController = PlayerController.Instance;
        if (playerController.PlayerCharacter == null) return;

        for (int i = 0; i < equipmentCount; i++)
        {
            // equipmentListにequipmentSlotを追加
            EquipmentSlot slot = Instantiate(equipmentSlot, equipmentList.transform);
            equipmentSlots.Add(slot);
            slot.gameObject.name = $"EquipmentSlot_{i + 1}";
        }
    }

    public void SetEquipments()
    {
        List<Equipment> equipmentList = PlayerController.Instance.PlayerCharacter.EquipmentList;
        if (equipmentList == null || equipmentList.Count == 0)
        {
            Debug.LogWarning("装備リストが空です。");
            return;
        }
        if (equipmentList.Count > equipmentSlots.Count)
        {
            Debug.LogWarning("装備スロットの数が装備リストの数より少ないです。");
            return;
        }
        for (int i = 0; i < equipmentList.Count; i++)
        {
            if (i < equipmentSlots.Count)
            {
                EquipmentSlot slot = equipmentSlots[i];
                Equipment equip = equipmentList[i];
                slot.setEquipment(equip);
            }
            else
            {
                Debug.LogWarning($"装備スロットの数が不足しています。装備リストのアイテム数: {equipmentList.Count}, 装備スロット数: {equipmentSlots.Count}");
                break;
            }
        }
    }
}
