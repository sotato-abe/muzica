using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TargetItemWindow : MonoBehaviour
{
    [SerializeField] ItemDetail itemDetail;
    [SerializeField] EquipmentStatusDisplay equipmentStatusDisplay;
    [SerializeField] PriceTag coinPriceTag;
    [SerializeField] PriceTag discPriceTag;

    private void Awake()
    {
        ClearTargetItem();
        equipmentStatusDisplay.gameObject.SetActive(false);
    }

    public void TargetItem(Item item, bool isOwn = true)
    {
        if (item == null)
        {
            ClearTargetItem();
            return;
        }

        itemDetail.SetItemDetail(item);

        if (coinPriceTag != null)
            coinPriceTag.SetPrice(item.Base.CoinPrice, isOwn);

        if (discPriceTag != null)
            discPriceTag.SetPrice(item.Base.DiscPrice, isOwn);

        if (item is Equipment equipment)
        {
            equipmentStatusDisplay.ShowEquipmentStatus(equipment);
            equipmentStatusDisplay.gameObject.SetActive(true);
        }
        else
        {
            equipmentStatusDisplay.gameObject.SetActive(false);
        }
    }

    private void ClearTargetItem()
    {
        itemDetail.ClearItemDetail();
        equipmentStatusDisplay.gameObject.SetActive(false);

        if (coinPriceTag != null)
            coinPriceTag.SetPrice(null);

        if (discPriceTag != null)
            discPriceTag.SetPrice(null);
    }
}
