using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Sprite emptyImage;
    [SerializeField] GameObject costList;
    [SerializeField] EquipmentSlotCounter equipmentSlotCounter;
    [SerializeField] EnegyCostIcon enegyCostIconPrefab;
    [SerializeField] GameObject counterArea;
    [SerializeField] GameObject counterList;

    private void Awake()
    {
        // 初期化処理
        SetEnpty();
        ClearCosts();
        counterArea.SetActive(false);
    }

    public void setEquipment(Equipment equipment)
    {
        if (equipment == null)
        {
            // 装備がnullの場合はデフォルトの画像を設定
            return;
        }
        // ここで装備スロットに装備アイテムを設定する処理を実装
        // 例えば、装備リストのアイテムをUIに表示するなど
        Debug.Log($"Setting equipment: {equipment.Base.Name}");
        image.sprite = equipment.Base.Sprite;
        SetCounters(equipment);
        SetCosts(equipment.EquipmentBase.EnegyCostList);
    }

    private void SetEnpty()
    {
        image.sprite = emptyImage;
    }

    private void SetCounters(Equipment equipment)
    {
        // 既存のカウンターを削除
        foreach (Transform child in counterList.transform)
        {
            Destroy(child.gameObject);
        }
        if (equipment == null) return;

        counterArea.SetActive(true);

        // 装備のコストを表示する処理
        foreach (var counter in equipment.EquipmentBase.EnegyCountList)
        {
            EquipmentSlotCounter newSlot = Instantiate(equipmentSlotCounter, counterList.transform);
            newSlot.SetCounter(counter);
        }
    }

    private void SetCosts(List<EnegyCost> enegyCosts)
    {
        ClearCosts();
        // EnegyCostを表示する処理
        foreach (var cost in enegyCosts)
        {
            EnegyCostIcon newIcon = Instantiate(enegyCostIconPrefab, costList.transform);
            newIcon.SetCost(cost);
        }
    }

    private void ClearCosts()
    {
        foreach (Transform child in costList.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
