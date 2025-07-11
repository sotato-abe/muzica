using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSubPanel : CharacterSubPanel
{
    [SerializeField] EnergyGauge energyGauge;

    public override void SetCharacter(Character player)
    {
        base.SetCharacter(player);
        // ここでプレイヤーのエネルギーゲージを初期化することもできます
        // energyGauge.SetLifeGauge(100, 100); // 例: 最大値100、現在値100

        energyGauge.SetLifeGauge(player.MaxLife, player.Life);
        energyGauge.SetBatteryGauge(player.MaxBattery, player.Battery);
        energyGauge.SetSoulGauge(100, player.Soul);
    }
}

