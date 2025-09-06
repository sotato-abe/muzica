using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConsumableCard : Card
{
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] GameObject attackCounterList;
    [SerializeField] GameObject enchantList;
    [SerializeField] AttackCounter attackCounterPrefab;
    [SerializeField] EnchantIcon enchantIconPrefab;

    public void SetConsumableDetail(Consumable consumable)
    {
        this.gameObject.SetActive(true);
        SetRarity(consumable.Base.Rarity);
        cardName.text = consumable.Base.Name;
        description.text = consumable.Base.Description;
        cardImage.sprite = consumable.Base.Sprite;
        cardImage.color = new Color(1, 1, 1, 1);
        SetEnchants(consumable.ConsumableBase.EnchantList);
        SetAttacks(consumable.ConsumableBase.EnergyAttackList);
        countText.text = consumable.UsableCount.ToString();
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
}
