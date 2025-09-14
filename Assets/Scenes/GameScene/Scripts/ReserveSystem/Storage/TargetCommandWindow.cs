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
    [SerializeField] GameObject currencyWindow;
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
        ShowPriceTags(command, isOwn);
    }

    private void ClearTargetCommand()
    {
        coinPriceTag.SetPrice(null);
        discPriceTag.SetPrice(null);
        currencyWindow.SetActive(false);
        commandCard.gameObject.SetActive(false);
    }

    private void ShowPriceTags(Command command, bool isOwn)
    {
        coinPriceTag.SetPrice(command.Base.CoinPrice, isOwn);
        discPriceTag.SetPrice(command.Base.DiscPrice, isOwn);

        if (command.Base.CoinPrice != null || command.Base.DiscPrice != null)
        {
            currencyWindow.SetActive(true);
        }
    }
}
