using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EquipmentWindow : MonoBehaviour
{
    private const int EQUIPMENT_COUNT = 2;
    [SerializeField] GameObject equipmentSlot;
    [SerializeField] GameObject equipmentList;
    PlayerController playerController;
    List<GameObject> equipmentSlots = new List<GameObject>();

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
            GameObject slot = Instantiate(equipmentSlot, equipmentList.transform);
            equipmentSlots.Add(slot);
            EquipmentArea slotComponent = slot.GetComponent<EquipmentArea>();

            if (slotComponent != null)
            {
                slotComponent.OnEquipAction += SetEquipments;
            }
            else
            {
                Debug.LogError("EquipmentArea が見つかりませんでした");
            }
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
                GameObject slot = equipmentSlots[i];
                Equipment equip = equipmentList[i];
                EquipmentSlot slotComponent = slot.GetComponent<EquipmentSlot>();
                slotComponent.setEquipment(equip);
            }
            else
            {
                Debug.LogWarning($"装備スロットの数が不足しています。装備リストのアイテム数: {equipmentList.Count}, 装備スロット数: {equipmentSlots.Count}");
                break;
            }
        }
    }

    public void Equip()
    {

    }
}
