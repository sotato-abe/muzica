using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TargetItemWindow : MonoBehaviour
{
    [SerializeField] ConsumableCard consumableCard;
    [SerializeField] EquipmentCard equipmentCard;
    [SerializeField] TreasureCard treasureCard;
    [SerializeField] PriceTag coinPriceTag;
    [SerializeField] PriceTag discPriceTag;
    [SerializeField] bool dispDetailSwitch = true;

    private void Awake()
    {
        ClearTargetItem();
        consumableCard.gameObject.SetActive(false);
        equipmentCard.gameObject.SetActive(false);
        treasureCard.gameObject.SetActive(false);
    }

    public void TargetItem(Item item, bool isOwn = true)
    {
        if (item == null)
        {
            ClearTargetItem();
            return;
        }
        if (item is Consumable consumable)
        {
            consumableCard.SetConsumableDetail(consumable);
            consumableCard.gameObject.SetActive(true);
        }
        else if (item is Equipment equipment)
        {
            equipmentCard.SetEquipmentDetail(equipment);
            equipmentCard.gameObject.SetActive(true);
        }
        else if (item is Treasure treasure)
        {
            treasureCard.SetTreasureDetail(treasure);
            treasureCard.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Unsupported item type: " + item.GetType());
            ClearTargetItem();
            return;
        }
        ShowDetail(item, isOwn);
    }

    private void ClearTargetItem()
    {
        if (coinPriceTag != null)
            coinPriceTag.SetPrice(null);

        if (discPriceTag != null)
            discPriceTag.SetPrice(null);

        consumableCard.gameObject.SetActive(false);
        equipmentCard.gameObject.SetActive(false);
        treasureCard.gameObject.SetActive(false);
    }

    private void ShowDetail(Item item, bool isOwn = true)
    {
        if (!dispDetailSwitch)
            return;

        if (coinPriceTag != null)
            coinPriceTag.SetPrice(item.Base.CoinPrice, isOwn);

        if (discPriceTag != null)
            discPriceTag.SetPrice(item.Base.DiscPrice, isOwn);
    }
}
