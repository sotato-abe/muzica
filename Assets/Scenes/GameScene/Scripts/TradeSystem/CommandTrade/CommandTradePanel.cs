using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class CommandTradePanel : Panel
{
    [SerializeField] TargetCommandWindow targetCommandWindow;
    [SerializeField] ShopCommandWindow shopCommandWindow;
    [SerializeField] BagCategory bagCategory;
    [SerializeField] StorageWindow storageWindow;
    [SerializeField] TableWindow tableWindow;

    private void Start()
    {
        shopCommandWindow.OnTargetCommand += TargetCommand;
        storageWindow.OnTargetCommand += TargetCommand;
        tableWindow.OnTargetCommand += TargetCommand;
        bagCategory.OnChangeWindow += ChangeWindow;
        ChangeWindow(true);
    }

    public void TargetCommand(Command Command)
    {
        targetCommandWindow.TargetCommand(Command);
    }

    public void ChangeWindow(bool isBag)
    {
        storageWindow.gameObject.SetActive(isBag);
        tableWindow.gameObject.SetActive(!isBag);
    }

    public void SetShopCommands(List<Command> Commands)
    {
        shopCommandWindow.SetCommands(Commands);
    }
}
