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
    [SerializeField] GameObject currencyWindow;
    [SerializeField] PriceTag coinPriceTag;
    [SerializeField] PriceTag discPriceTag;

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
            consumableCard.SetCard(item);
            consumableCard.gameObject.SetActive(true);
        }
        else if (item is Equipment equipment)
        {
            equipmentCard.SetCard(item);
            equipmentCard.gameObject.SetActive(true);
        }
        else if (item is Treasure treasure)
        {
            treasureCard.SetCard(item);
            treasureCard.gameObject.SetActive(true);
        }
        else
        {
            ClearTargetItem();
            return;
        }
        ShowPriceTags(item, isOwn);
    }

    private void ClearTargetItem()
    {
        coinPriceTag.SetPrice(null);
        discPriceTag.SetPrice(null);
        currencyWindow.SetActive(false);
        consumableCard.gameObject.SetActive(false);
        equipmentCard.gameObject.SetActive(false);
        treasureCard.gameObject.SetActive(false);
    }

    private void ShowPriceTags(Item item, bool isOwn = true)
    {

        coinPriceTag.SetPrice(item.Base.CoinPrice, isOwn);
        discPriceTag.SetPrice(item.Base.DiscPrice, isOwn);

        if (item.Base.CoinPrice > 0 || item.Base.DiscPrice > 0)
        {
            currencyWindow.SetActive(true);
        }
    }
}
