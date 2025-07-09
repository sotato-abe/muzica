using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ConsumableInfo : MonoBehaviour
{
    [SerializeField] TargetIcon targetIcon;
    [SerializeField] GameObject enchantList;
    [SerializeField] GameObject counterList;
    [SerializeField] EnchantIcon enchantIconPrefab;
    [SerializeField] AttackCounter attackCounterPrefab;

    public void SetInfo(Consumable consumable)
    {
        gameObject.SetActive(true);
        targetIcon.SetTargetType(consumable.ConsumableBase.TargetType);
        SetEnchants(consumable.ConsumableBase.EnchantList);
        SetAttacks(consumable.ConsumableBase.EnegyAttackList);
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
            AttackCounter newSlot = Instantiate(attackCounterPrefab, counterList.transform);
            newSlot.SetCounter(attack);
        }
    }

}