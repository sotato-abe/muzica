using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ItemLibraryWindows : SelectWindow
{
    [SerializeField] ItemListWindows itemListWindows;

    public override void Start()
    {
        itemListWindows.OnExitWindow += ExitTargetTypeWindow;
        ChangeActiveWindow(false);
        base.Start();
    }

    public override void WindowOpen()
    {
        base.WindowOpen();
        itemListWindows.WindowOpen();
    }

    public override void WindowClose()
    {
        base.WindowClose();
        itemListWindows.WindowClose();
    }

    public override void TargetElement(int index)
    {
        switch (index)
        {
            case 0:
                itemListWindows.ChangeItemType(ItemType.Consumable);
                break;
            case 1:
                itemListWindows.ChangeItemType(ItemType.Equipment);
                break;
            case 2:
                itemListWindows.ChangeItemType(ItemType.Treasure);
                break;
        }
    }

    public override void EnterTargetWindow()
    {
        ChangeActiveWindow(false);
        itemListWindows.ChangeActiveWindow(true);
    }

    public override void ExitWindow()
    {
        base.ExitWindow();
    }

    public void ExitTargetTypeWindow()
    {
        ChangeActiveWindow(true);
    }
}
