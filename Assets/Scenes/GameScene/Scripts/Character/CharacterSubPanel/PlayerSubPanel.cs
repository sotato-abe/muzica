using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSubPanel : CharacterSubPanel
{
    [SerializeField] private StatusText statusText;

    public override void SetCharacter(Character character)
    {
        base.SetCharacter(character);
        energyGauge.gameObject.SetActive(true);
        turnBar.gameObject.SetActive(false);
        SetEnergy();
        SetStatusText(null);
    }

    public void SetBattle()
    {
        turnBar.gameObject.SetActive(true);
    }

    public void SetStatusText(string status)
    {
        statusText.SetText(status);
    }
}

