using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PocketPanel : BattleActionPanel
{
    [SerializeField] TargetItemWindow targetItemWindow;
    [SerializeField] PocketWindow pocketWindow;

    private void Start()
    {
        pocketWindow.OnTargetItem += TargetItem;
    }

    public void TargetItem(Item item)
    {
        targetItemWindow.TargetItem(item);
    }
}
