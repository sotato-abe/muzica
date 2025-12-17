using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CommandCard : Card
{
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] GameObject costSpace;
    [SerializeField] GameObject enchantList;
    [SerializeField] GameObject attackSpace;

    [SerializeField] CostIconPrefab costIconPrefab;
    [SerializeField] AttackPrefab attackPrefab;
    [SerializeField] EnchantPrefab enchantPrefab;


    public override void SetCommand(Command command)
    {
        base.SetCommand(command);
        descriptionText.text = command.Base.Description;
        SetCost(command.Base.EnergyCostList);
        SetAttacks(command.Base.AttackList);
        SetEnchants(command.Base.EnchantList);
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
        foreach (var attack in attacks)
        {
            AttackPrefab attackPrefabInstance = Instantiate(attackPrefab, attackSpace.transform);
            attackPrefabInstance.SetAttack(attack);
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
            EnchantPrefab newEnchant = Instantiate(enchantPrefab, enchantList.transform);
            newEnchant.SetEnchant(enchant);
        }
    }
}
