using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Sprite emptyImage;
    [SerializeField] GameObject costList;
    [SerializeField] TargetIcon targetIcon;
    [SerializeField] EquipmentSlotCounter equipmentSlotCounter;
    [SerializeField] EnegyCostIcon enegyCostIconPrefab;
    [SerializeField] EnchantIcon enchantIconPrefab;
    [SerializeField] GameObject counterArea;
    [SerializeField] GameObject counterList;
    [SerializeField] GameObject enchantList;

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
        counterArea.SetActive(true);
        image.sprite = equipment.Base.Sprite;
        targetIcon.SetTargetType(equipment.EquipmentBase.TargetType);
        SetCosts(equipment.EquipmentBase.EnegyCostList);
        SetEnchants(equipment.EquipmentBase.EnchantList);
        SetAttacks(equipment.EquipmentBase.EnegyAttackList);
    }

    private void SetEnpty()
    {
        image.sprite = emptyImage;
    }

    private void SetCosts(List<EnegyCost> costs)
    {
        ClearCosts();
        // EnegyCostを表示する処理
        foreach (var cost in costs)
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

    private void SetEnchants(List<Enchant> enchants)
    {
        foreach (Transform child in enchantList.transform)
        {
            Destroy(child.gameObject);
        }

        // エンチャントを表示する処理
        foreach (var enchant in enchants)
        {
            EnchantIcon newIcon = Instantiate(enchantIconPrefab, enchantList.transform);
            newIcon.SetEnchant(enchant);
        }
    }

    private void SetAttacks(List<EnegyCount> enegyCountList)
    {
        // 既存のカウンターを削除
        foreach (Transform child in counterList.transform)
        {
            Destroy(child.gameObject);
        }
        // 装備のコストを表示する処理
        foreach (var attack in enegyCountList)
        {
            EquipmentSlotCounter newSlot = Instantiate(equipmentSlotCounter, counterList.transform);
            newSlot.SetCounter(attack);
        }
    }
}
