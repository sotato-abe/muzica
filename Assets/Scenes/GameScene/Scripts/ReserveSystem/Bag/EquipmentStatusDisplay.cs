using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EquipmentStatusDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI powerText;
    [SerializeField] TextMeshProUGUI techniqueText;
    [SerializeField] TextMeshProUGUI defenseText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI luckText;

    public void ShowEquipmentStatus(Equipment equipment)
    {
        if (equipment == null) return;

        powerText.text = equipment.EquipmentBase.Power.ToString();
        techniqueText.text = equipment.EquipmentBase.Technique.ToString();
        defenseText.text = equipment.EquipmentBase.Defense.ToString();
        speedText.text = equipment.EquipmentBase.Speed.ToString();
        luckText.text = equipment.EquipmentBase.Luck.ToString();
    }
}
