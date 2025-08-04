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
    [SerializeField] GameObject countList;
    [SerializeField] EnergyCostIcon energyCostIconPrefab;
    [SerializeField] GameObject countIconPrefab;

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
            SetCount(consumable.UsableCount);
        }
        else if (item is Equipment equipment)
        {
            equipmentInfo.SetInfo(equipment);
            SetCosts(equipment.EquipmentBase.EnergyCostList);
        }
        else if (item is Treasure treasure)
        {
            treasureInfo.SetInfo(treasure);
        }
        else
        {
            Debug.LogError("Unknown item type: " + item.Base.itemType);
            return;
        }
    }

    public void ClearItemDetail()
    {
        ResetSlot();
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

    private void SetCount(int count)
    {
        foreach (Transform child in countList.transform)
        {
            Destroy(child.gameObject);
        }
        for(int i = 0; i < count; i++)
        {
            Instantiate(countIconPrefab, countList.transform);
        }
    }

    public void ResetSlot()
    {
        itemImage.color = new Color(1, 1, 1, 0);

        foreach (Transform child in costList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in countList.transform)
        {
            Destroy(child.gameObject);
        }
        equipmentInfo.gameObject.SetActive(false);
        consumableInfo.gameObject.SetActive(false);
        treasureInfo.gameObject.SetActive(false);
    }
}
