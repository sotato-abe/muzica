using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConsumableCard : Card
{
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] AttackPrefab attackPrefab;
    [SerializeField] EnchantPrefab enchantPrefab;

    [SerializeField] GameObject attackSpace;
    [SerializeField] GameObject enchantSpace;

    private Consumable currentConsumable;

    public override void SetCard(Item item)
    {
        base.SetCard(item);
        currentConsumable = item as Consumable;
        description.text = currentConsumable.Base.Description;
        countText.text = currentConsumable.UsableCount.ToString();
        SetAttack(currentConsumable.ConsumableBase.Attack);
        SetEnchants(currentConsumable.ConsumableBase.EnchantList);
    }

    private void SetAttack(Attack attack)
    {
        foreach (Transform child in attackSpace.transform)
        {
            Destroy(child.gameObject);
        }
        AttackPrefab attackPrefabInstance = Instantiate(attackPrefab, attackSpace.transform);
        attackPrefabInstance.SetAttack(attack);
    }

    private void SetEnchants(List<Enchant> enchants)
    {
        foreach (Transform child in enchantSpace.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var enchant in enchants)
        {
            EnchantPrefab newEnchant = Instantiate(enchantPrefab, enchantSpace.transform);
            newEnchant.SetEnchant(enchant);
        }
    }

    public TotalAttack GetTotalAttack()
    {
        TotalAttack totalAttack = new TotalAttack();
        totalAttack.AttackList = currentConsumable.ConsumableBase.Attack != null ? new List<Attack> { currentConsumable.ConsumableBase.Attack } : new List<Attack>();
        totalAttack.EnchantList = currentConsumable.ConsumableBase.EnchantList;
        return totalAttack;
    }
}
