using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentDetail : MonoBehaviour
{
    [SerializeField] public GameObject blockSlot;
    [SerializeField] TargetIcon targetIcon;
    [SerializeField] GameObject costList;
    [SerializeField] EquipmentInfo equipmentInfo;
    [SerializeField] public ItemBlock itemBlockPrefab;
    [SerializeField] EnegyCostIcon enegyCostIconPrefab;

    private void Awake()
    {
        // 初期化処理
        ResetSlot();
    }

    public void SetEquipment(Equipment equipment)
    {
        if (equipment == null)
        {
            ResetSlot();
            return;
        }
        SetEquipmentBlock(equipment);
        equipmentInfo.SetInfo(equipment);
        SetCosts(equipment.EquipmentBase.EnegyCostList);
    }

    public virtual void SetEquipmentBlock(Equipment equipment)
    {
        foreach (Transform child in blockSlot.transform)
        {
            Destroy(child.gameObject);
        }
        // 装備アイテムのブロックを設定
        ItemBlock itemBlock = Instantiate(itemBlockPrefab, blockSlot.transform);
        itemBlock.Setup(equipment, this.transform);
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
        equipmentInfo.gameObject.SetActive(false);
    }
}
