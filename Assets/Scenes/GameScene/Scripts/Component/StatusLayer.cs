using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusLayer : MonoBehaviour
{
    [SerializeField] EnergyStatusBar soulStatusBar;
    [SerializeField] EnergyStatusBar lifeStatusBar;
    [SerializeField] EnergyStatusBar batteryStatusBar;
    [SerializeField] StatusText levelText;
    [SerializeField] TextMeshProUGUI powerText;
    [SerializeField] TextMeshProUGUI defenseText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI luckText;

    [SerializeField] GameObject equipArea;

    public void SetCharacterStatus(Character character)
    {
        soulStatusBar.SetEnergy(100, character.Soul);
        lifeStatusBar.SetEnergy(character.Base.MaxLife, character.Life);
        batteryStatusBar.SetEnergy(character.Base.MaxBattery, character.Battery);

        levelText.SetText(character.Level.ToString());
        powerText.text = character.Power.ToString();
        defenseText.text = character.Defense.ToString();
        speedText.text = character.Speed.ToString();
        luckText.text = character.Luck.ToString();
    }
}
