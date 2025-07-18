using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PocketPanel : Panel
{
    [SerializeField] TargetItemWindow targetItemWindow;
    [SerializeField] PocketWindow pocketWindow;
    public bool canExecuteActionFlg = false;

    private void Start()
    {
        pocketWindow.OnTargetItem += TargetItem;
    }

    public void TargetItem(Item item)
    {
        targetItemWindow.TargetItem(item);
    }

    public void CanExecuteAction(bool canExecute)
    {
        canExecuteActionFlg = canExecute;
    }
}
