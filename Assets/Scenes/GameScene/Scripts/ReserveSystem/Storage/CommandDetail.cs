using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CommandDetail : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI commandNameText;
    [SerializeField] TextMeshProUGUI commandDescriptionText;
    [SerializeField] Image commandImage;
    [SerializeField] GameObject enchantList;
    [SerializeField] GameObject costList;
    [SerializeField] EnchantIcon enchantIconPrefab;
    [SerializeField] EnergyCostIcon energyCostIconPrefab;

    public void SetCommandDetail(Command command)
    {
        this.gameObject.SetActive(true);
        commandNameText.text = command.Base.Name;
        commandDescriptionText.text = command.Base.Description;
        commandImage.sprite = command.Base.Sprite;
        commandImage.color = new Color(1, 1, 1, 1);
        SetEnchants(command.Base.EnchantList);
        SetCosts(command.Base.EnergyCostList);
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

    private void SetCosts(List<EnergyCost> costs)
    {
        foreach (Transform child in costList.transform)
        {
            Destroy(child.gameObject);
        }

        // EnergyCostを表示する処理
        foreach (var cost in costs)
        {
            EnergyCostIcon newIcon = Instantiate(energyCostIconPrefab, costList.transform);
            newIcon.SetCost(cost);
        }
    }

    public void ResetSlot()
    {
        commandImage.color = new Color(1, 1, 1, 0);

        foreach (Transform child in costList.transform)
        {
            Destroy(child.gameObject);
        }
        this.gameObject.SetActive(false);
    }
}
