using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSubPanel : CharacterSubPanel
{

    public override void SetCharacter(Character character)
    {
        base.SetCharacter(character);
        energyGauge.gameObject.SetActive(true);
        turnBar.gameObject.SetActive(false);
        SetEnergy();
    }

    public void SetBattle()
    {
        turnBar.gameObject.SetActive(true);
    }
}

