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

    public delegate void TargetItemDelegate(Item? item, bool isOwn = true);
    public event TargetItemDelegate OnTargetItem;

    private void Awake()
    {
        SetEquipments();
        equipmentSlot1.OnTargetItem += TargetItem;
        equipmentSlot2.OnTargetItem += TargetItem;
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
            Debug.LogWarning("アイテムを装備していません。");
            equipmentSlot1.ResetSlot();
            equipmentSlot2.ResetSlot();
        }
    }

    public void UpdateInventory()
    {
        OnUpdateInventory?.Invoke();
    }

    public void TargetItem(Item item, bool isOwn = true)
    {
        if (item == null)
        {
            OnTargetItem?.Invoke(null);
            return;
        }
        OnTargetItem?.Invoke(item, isOwn);
    }
}
