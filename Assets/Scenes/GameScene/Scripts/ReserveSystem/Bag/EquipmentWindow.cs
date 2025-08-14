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

    public delegate void TargetItemDelegate(Item item, bool isOwn = true);
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
        equipmentSlot1.SetEquipment(PlayerController.Instance.PlayerCharacter.RightHandEquipment);
        equipmentSlot2.SetEquipment(PlayerController.Instance.PlayerCharacter.LeftHandEquipment);
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
