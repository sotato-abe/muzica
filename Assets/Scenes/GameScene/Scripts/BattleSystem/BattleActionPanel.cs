using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleActionPanel : Window
{
    public bool canExecuteActionFlg = false;

    public virtual void ChangeExecuteActionFlg(bool canExecute)
    {
        canExecuteActionFlg = canExecute;
    }
}
