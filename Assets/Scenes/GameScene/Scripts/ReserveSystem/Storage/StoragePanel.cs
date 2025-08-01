using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StoragePanel : Panel
{
    [SerializeField] StorageWindow storageWindow;
    [SerializeField] SlotSettingWindow slotSettingWindow;
    [SerializeField] TargetCommandWindow targetCommandWindow;

    private void Start()
    {
        storageWindow.OnTargetCommand += TargetCommand;
        slotSettingWindow.OnTargetCommand += TargetCommand;
    }

    public void TargetCommand(Command? command, bool isOwn = true)
    {
        targetCommandWindow.TargetCommand(command, isOwn);
    }
}
