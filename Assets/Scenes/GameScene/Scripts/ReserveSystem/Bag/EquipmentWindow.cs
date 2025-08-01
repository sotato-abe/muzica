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
    [SerializeField] EquipmentSlot equipmentSlot1;
    [SerializeField] EquipmentSlot equipmentSlot2;

    private void Awake()
    {
        SetEquipments();
    }

    private void OnEnable()
    {
        SetEquipments();
    }

    public void SetEquipments()
    {
        List<Equipment> equipmentList = PlayerController.Instance.PlayerCharacter.EquipmentList;

        if (equipmentList.Count == 1)
        {
            equipmentSlot1.SetEquipment(equipmentList[0]);
            equipmentSlot2.ResetSlot();
        }
        else if (equipmentList.Count == 2)
        {
            equipmentSlot1.SetEquipment(equipmentList[0]);
            equipmentSlot2.SetEquipment(equipmentList[1]);
        }
        else
        {
            Debug.LogWarning("装備アイテムの数が不正です。1つまたは2つの装備アイテムが必要です。");
            equipmentSlot1.ResetSlot();
            equipmentSlot2.ResetSlot();
        }
    }

    public void UpdateInventory()
    {
        OnUpdateInventory?.Invoke();
    }
}
