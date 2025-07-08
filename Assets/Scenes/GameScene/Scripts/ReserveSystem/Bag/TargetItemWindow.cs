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

    public void TargetItem(Item? item)
    {
        if (item == null)
        {
            ClearTargetItem();
            return;
        }

        itemDetail.SetItemDetail(item);
    }

    private void ClearTargetItem()
    {
        itemDetail.ClearItemDetail();
    }
}
