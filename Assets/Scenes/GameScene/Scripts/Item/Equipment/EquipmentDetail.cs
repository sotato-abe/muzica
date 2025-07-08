using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentDetail : MonoBehaviour
{
    [SerializeField] Image blockSlot;
    [SerializeField] ItemBlock itemBlockPrefab;
    [SerializeField] GameObject costList;
    [SerializeField] TargetIcon targetIcon;
    [SerializeField] EquipmentSlotCounter equipmentSlotCounter;
    [SerializeField] EnegyCostIcon enegyCostIconPrefab;
    [SerializeField] EnchantIcon enchantIconPrefab;
    [SerializeField] GameObject counterArea;
    [SerializeField] GameObject counterList;
    [SerializeField] GameObject enchantList;

    private void Awake()
    {
        // 初期化処理
        ResetSlot();
    }

    public void SetEquipment(Equipment equipment)
    {
        if (equipment == null)
        {
            // 装備がnullの場合はデフォルトの画像を設定
            return;
        }
        // ここで装備スロットに装備アイテムを設定する処理を実装
        // 例えば、装備リストのアイテムをUIに表示するなど
        SetEquipmentBlock(equipment);
        counterArea.SetActive(true);
        targetIcon.SetTargetType(equipment.EquipmentBase.TargetType);
        SetEnchants(equipment.EquipmentBase.EnchantList);
        SetAttacks(equipment.EquipmentBase.EnegyAttackList);
        SetCosts(equipment.EquipmentBase.EnegyCostList);
    }

    private void SetEquipmentBlock(Equipment equipment)
    {
        foreach (Transform child in blockSlot.transform)
        {
            Destroy(child.gameObject);
        }
        // 装備アイテムのブロックを設定
        ItemBlock itemBlock = Instantiate(itemBlockPrefab, blockSlot.transform);
        itemBlock.OnDropItem += DropItem;
        itemBlock.OnBagInItem += MoveBagInItem;
        itemBlock.Setup(equipment);
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
            EquipmentSlotCounter newSlot = Instantiate(equipmentSlotCounter, counterList.transform);
            newSlot.SetCounter(attack);
        }
    }

    public void MoveBagInItem(ItemBlock itemBlock)
    {
        // アイテムをドロップする処理
        if (itemBlock == null || itemBlock.Item == null) return;

        Item item = itemBlock.Item;
        PlayerController.Instance.BagInItemFromEquip(item);
        Destroy(itemBlock.gameObject);
        ResetSlot();
    }

    public void DropItem(ItemBlock itemBlock)
    {
        // アイテムをドロップする処理
        if (itemBlock == null || itemBlock.Item == null) return;

        Item item = itemBlock.Item;
        PlayerController.Instance.DropItemFromEquip(item);
        Destroy(itemBlock.gameObject);
        ResetSlot();
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
