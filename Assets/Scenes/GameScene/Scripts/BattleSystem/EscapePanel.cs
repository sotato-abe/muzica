using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EscapePanel : Panel
{
    public bool canExecuteActionFlg = false;

    public void CanExecuteAction(bool canExecute)
    {
        canExecuteActionFlg = canExecute;
    }
}
