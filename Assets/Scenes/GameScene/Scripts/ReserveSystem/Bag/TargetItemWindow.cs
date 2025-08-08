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
    [SerializeField] PriceTag coinPriceTag;
    [SerializeField] PriceTag discPriceTag;

    private void Awake()
    {
        ClearTargetItem();
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
    }

    private void ClearTargetItem()
    {
        itemDetail.ClearItemDetail();
        
        if (coinPriceTag != null)
            coinPriceTag.SetPrice(null);

        if (discPriceTag != null)
            discPriceTag.SetPrice(null);
    }
}
