using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TargetCommandWindow : MonoBehaviour
{
    [SerializeField] CommandCard commandCard;
    [SerializeField] PriceTag coinPriceTag;
    [SerializeField] PriceTag discPriceTag;

    private void Awake()
    {
        ClearTargetCommand();
        commandCard.gameObject.SetActive(false);
    }

    public void TargetCommand(Command command, bool isOwn = true)
    {
        if (command == null)
        {
            ClearTargetCommand();
            return;
        }

        commandCard.SetCommandCard(command);
        commandCard.gameObject.SetActive(true);
        if (coinPriceTag != null)
            coinPriceTag.SetPrice(command.Base.CoinPrice, isOwn);

        if (discPriceTag != null)
            discPriceTag.SetPrice(command.Base.DiscPrice, isOwn);
    }

    private void ClearTargetCommand()
    {
        commandCard.gameObject.SetActive(false);

        if (coinPriceTag != null)
            coinPriceTag.SetPrice(null);

        if (discPriceTag != null)
            discPriceTag.SetPrice(null);
    }
}
