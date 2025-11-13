using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class BagWindow : SlidePanel
{
    [SerializeField] CategorySwitch categorySwitch;
    [SerializeField] ItemBoxWindow itemBoxWindow;
    [SerializeField] CommandBoxWindow commandBoxWindow;

    public delegate void TargetItemDelegate(Item item, bool isOwn = true);
    public event TargetItemDelegate OnTargetItem;

    public delegate void TargetCommandDelegate(Command Command, bool isOwn = true);
    public event TargetCommandDelegate OnTargetCommand;

    public void Start()
    {
        itemBoxWindow.OnTargetItem += TargetItem;
        commandBoxWindow.OnTargetCommand += TargetCommand;
        categorySwitch.OnChangeWindow += ChangeWindow;
        ChangeWindow(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            categorySwitch.SwitchActiveButton();
        }
    }

    public void TargetItem(Item item, bool isOwn = true)
    {
        OnTargetItem?.Invoke(item, isOwn);
    }

    public void TargetCommand(Command Command, bool isOwn = true)
    {
        OnTargetCommand?.Invoke(Command, isOwn);
    }

    public void ChangeWindow(bool isBag)
    {
        itemBoxWindow.gameObject.SetActive(isBag);
        commandBoxWindow.gameObject.SetActive(!isBag);
    }

    public void SetupBagUI()
    {
        itemBoxWindow.SetItems();
        commandBoxWindow.SetCommands();
    }
}
