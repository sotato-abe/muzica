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
    [SerializeField] EquipCardSlot equipCardSlot1;
    [SerializeField] EquipCardSlot equipCardSlot2;

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
        Equipment RightHandEquipment = PlayerController.Instance.PlayerCharacter.RightHandEquipment;
        Equipment LeftHandEquipment = PlayerController.Instance.PlayerCharacter.LeftHandEquipment;
        equipCardSlot1.SetEquip(RightHandEquipment);
        equipCardSlot2.SetEquip(LeftHandEquipment);
    }

    public void UpdateInventory()
    {
        OnUpdateInventory?.Invoke();
    }
}
