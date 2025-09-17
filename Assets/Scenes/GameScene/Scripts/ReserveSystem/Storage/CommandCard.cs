using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CommandCard : MonoBehaviour
{
    [SerializeField] RarityIcon cardRarity;
    [SerializeField] TextMeshProUGUI cardName;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] public Image cardFrame;
    [SerializeField] Image cardImage;
    [SerializeField] GameObject enchantList;
    [SerializeField] GameObject attackCounterList;
    [SerializeField] AttackCounter attackCounterPrefab;
    [SerializeField] EnchantIcon enchantIconPrefab;
    [SerializeField] EnergyCostIcon energyCostIconPrefab;

    public void SetCommandCard(Command command)
    {
        this.gameObject.SetActive(true);
        cardRarity.SetRarityIcon(command.Base.Rarity);
        cardName.text = command.Base.Name;
        descriptionText.text = command.Base.Description;
        cardImage.sprite = command.Base.Sprite;
        cardImage.color = new Color(1, 1, 1, 1);
        SetAttacks(command.Base.EnergyAttackList);
        SetEnchants(command.Base.EnchantList);
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
}
