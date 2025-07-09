using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetail : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] EquipmentInfo equipmentInfo;
    [SerializeField] ConsumableInfo consumableInfo;
    [SerializeField] TreasureInfo treasureInfo;
    [SerializeField] GameObject costList;
    [SerializeField] EnegyCostIcon enegyCostIconPrefab;

    private void Awake()
    {
        ResetSlot();
    }

    public void SetItemDetail(Item item)
    {
        itemImage.sprite = item.Base.Sprite;
        itemImage.color = new Color(1, 1, 1, 1);
        if (item is Consumable consumable)
        {
            consumableInfo.SetInfo(consumable);
        }
        else if (item is Equipment equipment)
        {
            equipmentInfo.SetInfo(equipment);
            SetCosts(equipment.EquipmentBase.EnegyCostList);
        }
        else if (item is Treasure treasure)
        {
            treasureInfo.SetInfo(treasure);
        }
        else
        {
            Debug.LogError("Unknown item type: " + item.GetType());
            return;
        }
    }

    public void ClearItemDetail()
    {
        ResetSlot();
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
        itemImage.color = new Color(1, 1, 1, 0);

        foreach (Transform child in costList.transform)
        {
            Destroy(child.gameObject);
        }
        equipmentInfo.gameObject.SetActive(false);
        consumableInfo.gameObject.SetActive(false);
        treasureInfo.gameObject.SetActive(false);
    }
}
