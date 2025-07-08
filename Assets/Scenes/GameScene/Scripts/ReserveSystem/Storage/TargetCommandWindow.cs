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

    private void Awake()
    {
        ClearTargetCommand();
    }

    public void TargetCommand(Command? command)
    {
        if (command == null)
        {
            ClearTargetCommand();
            return;
        }

        commandDetail.SetCommandDetail(command);
    }

    private void ClearTargetCommand()
    {
        commandDetail.ResetSlot();
    }
}
