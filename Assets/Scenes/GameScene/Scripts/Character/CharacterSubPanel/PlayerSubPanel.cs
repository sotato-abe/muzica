using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerSubPanel : BattleCharacterSubPanel
{
    [SerializeField] private StatusText statusText;

    #region Character Setup
    public void SetPlayer(Character character)
    {
        base.SetCharacter(character);
        energyGauge.gameObject.SetActive(true);
        turnBar.gameObject.SetActive(false);
        SetEnergy();
        SetStatusText(null);
    }
    #endregion

    public void SetBattle()
    {
        turnBar.gameObject.SetActive(true);
    }

    public void SetStatusText(string status)
    {
        statusText.SetText(status);
    }

    public override void SetActive(bool activeFlg, Action onComplete = null)
    {
        fixedDisplayFlg = activeFlg;
        if (!activeFlg)
        {
            StopAllCoroutines();
            turnBar.gameObject.SetActive(false);
            blowingPanel.gameObject.SetActive(false);
        }
        this.gameObject.SetActive(true);
        base.SetActive(activeFlg, onComplete);
    }
}

