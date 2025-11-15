using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CharacterStatus : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI lifeValText;
    [SerializeField] TextMeshProUGUI batteryValText;
    [SerializeField] TextMeshProUGUI powerValText;
    [SerializeField] TextMeshProUGUI defenseValText;
    [SerializeField] TextMeshProUGUI techniqueValText;
    [SerializeField] TextMeshProUGUI speedValText;
    [SerializeField] TextMeshProUGUI luckValText;

    public void SetCharacter(Character character)
    {
        lifeValText.text = character.Life.ToString();
        batteryValText.text = character.Battery.ToString();
        powerValText.text = character.Power.ToString();
        defenseValText.text = character.Defense.ToString();
        techniqueValText.text = character.Technique.ToString();
        speedValText.text = character.Speed.ToString();
        luckValText.text = character.Luck.ToString();
    }
}
