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
}

