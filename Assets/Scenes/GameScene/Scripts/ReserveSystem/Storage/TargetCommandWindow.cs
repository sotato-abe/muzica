using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TargetCommandWindow : MonoBehaviour
{
    [SerializeField] CommandDetail commandDetail;
    [SerializeField] PriceTag coinPriceTag;
    [SerializeField] PriceTag discPriceTag;

    private void Awake()
    {
        ClearTargetCommand();
    }

    public void TargetCommand(Command? command, bool isOwn = true)
    {
        if (command == null)
        {
            ClearTargetCommand();
            return;
        }

        commandDetail.SetCommandDetail(command);
        if (coinPriceTag != null)
            coinPriceTag.SetPrice(command.Base.CoinPrice, isOwn);

        if (discPriceTag != null)
            discPriceTag.SetPrice(command.Base.DiscPrice, isOwn);
    }

    private void ClearTargetCommand()
    {
        commandDetail.ResetSlot();

        if (coinPriceTag != null)
            coinPriceTag.SetPrice(null);

        if (discPriceTag != null)
            discPriceTag.SetPrice(null);
    }
}
