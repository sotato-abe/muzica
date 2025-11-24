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

    private List<EnergyCost> energyCostList = new List<EnergyCost>();
    private List<Attack> attackList = new List<Attack>();
    private List<Enchant> enchantList = new List<Enchant>();

    public override void SetCard(Item item)
    {
        base.SetCard(item);
        Equipment equipment = item as Equipment;
        currentEquipment = equipment;
        ResetCurrentCard();
    }


    public void ResetCurrentCard()
    {
        energyCostList.Clear();     
        attackList.Clear();
        enchantList.Clear();
        
        // ディープコピーを作成して元のEquipmentBaseの値を保護する
        energyCostList = new List<EnergyCost>();
        foreach (var cost in currentEquipment.EquipmentBase.EnergyCostList)
        {
            energyCostList.Add(new EnergyCost(cost.type, cost.val));
        }
        
        attackList = new List<Attack>();
        foreach (var attack in currentEquipment.EquipmentBase.AttackList)
        {
            attackList.Add(new Attack(attack)); // Attackのコピーコンストラクタを使用
        }
        
        enchantList = new List<Enchant>();
        foreach (var enchant in currentEquipment.EquipmentBase.EnchantList)
        {
            enchantList.Add(new Enchant(enchant)); // Enchantにはコピーコンストラクタが既に存在
        }
        
        SetCost();
        SetAttacks();
        SetEnchants();
    }

    private void SetCost()
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

    private void SetAttacks()
    {
        foreach (Transform child in attackSpace.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var attack in attackList)
        {
            AttackPrefab attackPrefabInstance = Instantiate(attackPrefab, attackSpace.transform);
            attackPrefabInstance.SetAttack(attack);
        }
    }

    private void SetEnchants()
    {

        foreach (Transform child in enchantSpace.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var enchant in enchantList)
        {
            EnchantPrefab newEnchant = Instantiate(enchantPrefab, enchantSpace.transform);
            newEnchant.SetEnchant(enchant);
        }
    }

    public void CommandUpdate(Command command)
    {
        if (command == null) return;
        // 一致するAttackがあったらカウントを増やす
        // 一致するEnchantがあったらカウントを増やす
        TotalAttack totalAttack = command.GetTotalAttack();
        foreach (var totalAttackItem in totalAttack.AttackList)
        {
            foreach (var attack in attackList)
            {
                if (attack.AttackType == totalAttackItem.AttackType)
                {
                    attack.AttackUpdate(totalAttackItem);
                }
            }
        }

        foreach (var totalEnchantItem in totalAttack.EnchantList)
        {
            foreach (var enchant in enchantList)
            {
                if (enchant.Type == totalEnchantItem.Type)
                {
                    enchant.EnchantUpdate(totalEnchantItem);
                }
            }
        }
        SetAttacks();
        SetEnchants();
    }

    public TotalAttack GetTotalAttack()
    {
        TotalAttack totalAttack = new TotalAttack();
        totalAttack.AttackList = attackList;
        totalAttack.EnchantList = enchantList;
        return totalAttack;
    }
}
