using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentDetail : MonoBehaviour
{
    [SerializeField] public GameObject blockSlot;
    [SerializeField] TargetIcon targetIcon;
    [SerializeField] GameObject costList;
    [SerializeField] GameObject counterArea;
    [SerializeField] GameObject counterList;
    [SerializeField] GameObject enchantList;
    [SerializeField] public ItemBlock itemBlockPrefab;
    [SerializeField] EquipmentAttackCounter attackCounterPrefab;
    [SerializeField] EnegyCostIcon enegyCostIconPrefab;
    [SerializeField] EnchantIcon enchantIconPrefab;

    private void Awake()
    {
        // 初期化処理
        ResetSlot();
    }

    public void SetEquipment(Equipment equipment)
    {
        if (equipment == null)
        {
            ResetSlot();
            return;
        }
        SetEquipmentBlock(equipment);
        counterArea.SetActive(true);
        targetIcon.SetTargetType(equipment.EquipmentBase.TargetType);
        SetEnchants(equipment.EquipmentBase.EnchantList);
        SetAttacks(equipment.EquipmentBase.EnegyAttackList);
        SetCosts(equipment.EquipmentBase.EnegyCostList);
    }

    public virtual void SetEquipmentBlock(Equipment equipment)
    {
        foreach (Transform child in blockSlot.transform)
        {
            Destroy(child.gameObject);
        }
        // 装備アイテムのブロックを設定
        ItemBlock itemBlock = Instantiate(itemBlockPrefab, blockSlot.transform);
        itemBlock.Setup(equipment, this.transform);
    }

    private void SetCosts(List<EnegyCost> costs)
    {
        foreach (Transform child in costList.transform)
        {
            Destroy(child.gameObject);
        }

        // EnegyCostを表示する処理
        foreach (var cost in costs)
        {
            EnegyCostIcon newIcon = Instantiate(enegyCostIconPrefab, costList.transform);
            newIcon.SetCost(cost);
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
            EquipmentAttackCounter newSlot = Instantiate(attackCounterPrefab, counterList.transform);
            newSlot.SetCounter(attack);
        }
    }

    public void ResetSlot()
    {
        foreach (Transform child in blockSlot.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in costList.transform)
        {
            Destroy(child.gameObject);
        }
        counterArea.SetActive(false);
    }
}
