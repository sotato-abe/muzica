using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentCard : Card
{
    [SerializeField] TextMeshProUGUI powerText;
    [SerializeField] TextMeshProUGUI techniqueText;
    [SerializeField] TextMeshProUGUI defenseText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI luckText;
    [SerializeField] GameObject attackCounterList;
    [SerializeField] GameObject enchantList;
    [SerializeField] GameObject costList;
    [SerializeField] AttackCounter attackCounterPrefab;
    [SerializeField] EnchantIcon enchantIconPrefab;
    [SerializeField] CostIconPrefab costIconPrefab;

    public void SetEquipmentDetail(Equipment equipment)
    {
        this.gameObject.SetActive(true);
        SetRarity(equipment.Base.Rarity);
        cardName.text = equipment.Base.Name;
        cardImage.sprite = equipment.Base.Sprite;
        cardImage.color = new Color(1, 1, 1, 1);
        SetCost(equipment.EquipmentBase.EnergyCostList);
        SetEnchants(equipment.EquipmentBase.EnchantList);
        SetAttacks(equipment.EquipmentBase.EnergyAttackList);
        SetStatus(equipment);
    }

    private void SetCost(List<EnergyCost> energyCostList)
    {
        foreach (Transform child in costList.transform)
        {
            Destroy(child.gameObject);
        }

        // Costを表示する処理
        foreach (var cost in energyCostList)
        {
            CostIconPrefab newCost = Instantiate(costIconPrefab, costList.transform);
            newCost.SetCostIcon(cost);
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
            EnchantIcon newEnchant = Instantiate(enchantIconPrefab, enchantList.transform);
            newEnchant.SetEnchant(enchant);
        }
    }

    private void SetAttacks(List<EnergyCount> counts)
    {
        foreach (Transform child in attackCounterList.transform)
        {
            Destroy(child.gameObject);
        }

        // EnergyCostを表示する処理
        foreach (var count in counts)
        {
            AttackCounter newCounter = Instantiate(attackCounterPrefab, attackCounterList.transform);
            newCounter.SetCounter(count);
        }
    }

    private void SetStatus(Equipment equipment)
    {
        powerText.text = ConvertStatusText(equipment.EquipmentBase.Power);
        techniqueText.text = ConvertStatusText(equipment.EquipmentBase.Technique);
        defenseText.text = ConvertStatusText(equipment.EquipmentBase.Defense);
        speedText.text = ConvertStatusText(equipment.EquipmentBase.Speed);
        luckText.text = ConvertStatusText(equipment.EquipmentBase.Luck);
    }

    private string ConvertStatusText(int val)
    {
        return 0 < val ? val.ToString() : "-";
    }
}
