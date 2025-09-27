using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentTypeIcon : MonoBehaviour
{
    [SerializeField] Image targetIcon;
    [SerializeField] Sprite weaponIcon;
    [SerializeField] Sprite deviceIcon;
    [SerializeField] Sprite armorIcon;

    public void SetEquipmentType(EquipmentType equipmentType)
    {
        switch (equipmentType)
        {
            case EquipmentType.Weapon:
                targetIcon.sprite = weaponIcon;
                break;
            case EquipmentType.Device:
                targetIcon.sprite = deviceIcon;
                break;
            case EquipmentType.Armor:
                targetIcon.sprite = armorIcon;
                break;
            default:
                targetIcon.sprite = null;
                break;
        }
    }
}
