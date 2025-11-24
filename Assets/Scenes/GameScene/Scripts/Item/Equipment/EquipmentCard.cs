using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentCard : Card
{
    [SerializeField] CostIconPrefab costIconPrefab;
    [SerializeField] AttackPrefab attackPrefab;
    [SerializeField] EnchantPrefab enchantPrefab;

    [SerializeField] GameObject costSpace;
    [SerializeField] GameObject attackSpace;
    [SerializeField] GameObject enchantSpace;

    private Equipment currentEquipment;

    private List<Attack> attackList = new List<Attack>();
    private List<Enchant> enchantList = new List<Enchant>();

    public override void SetCard(Item item)
    {
        base.SetCard(item);
        Equipment equipment = item as Equipment;
        currentEquipment = equipment;
        SetCost(equipment.EquipmentBase.EnergyCostList);
        SetAttacks(equipment.EquipmentBase.AttackList);
        SetEnchants(equipment.EquipmentBase.EnchantList);
    }

    private void SetCost(List<EnergyCost> energyCostList)
    {
        foreach (Transform child in costSpace.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var cost in energyCostList)
        {
            CostIconPrefab newCost = Instantiate(costIconPrefab, costSpace.transform);
            newCost.SetCostIcon(cost);
        }
    }

    private void SetAttacks(List<Attack> attacks)
    {
        foreach (Transform child in attackSpace.transform)
        {
            Destroy(child.gameObject);
        }
        attackList.Clear();
        foreach (var attack in attacks)
        {
            AttackPrefab attackPrefabInstance = Instantiate(attackPrefab, attackSpace.transform);
            attackPrefabInstance.SetAttack(attack);
            attackList.Add(attack);
        }
    }

    private void SetEnchants(List<Enchant> enchants)
    {

        foreach (Transform child in enchantSpace.transform)
        {
            Destroy(child.gameObject);
        }
        enchantList.Clear();
        foreach (var enchant in enchants)
        {
            EnchantPrefab newEnchant = Instantiate(enchantPrefab, enchantSpace.transform);
            newEnchant.SetEnchant(enchant);
            enchantList.Add(enchant);
        }
    }

    public void CommandUpdate(Command command)
    {
        // TODO : コマンドによる強化を実装
        // 一致するAttackがあったらカウントを増やす
        // 一致するEnchantがあったらカウントを増やす

    }

    public TotalAttack GetTotalAttack()
    {
        TotalAttack totalAttack = new TotalAttack();
        totalAttack.AttackList = attackList;
        totalAttack.EnchantList = enchantList;
        return totalAttack;
    }

    public void ResetCard()
    {
        SetCost(currentEquipment.EquipmentBase.EnergyCostList);
        SetAttacks(currentEquipment.EquipmentBase.AttackList);
        SetEnchants(currentEquipment.EquipmentBase.EnchantList);
    }

    public List<Attack> GetAttackList()
    {
        return currentEquipment.EquipmentBase.AttackList;
    }

    public List<Enchant> GetEnchantList()
    {
        return currentEquipment.EquipmentBase.EnchantList;
    }
}
