using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetIcon : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] Sprite selfSprite;
    [SerializeField] Sprite randomSprite;

    [SerializeField] Sprite individualAttackSprite;
    [SerializeField] Sprite groupAttackSprite;
    [SerializeField] Sprite allAttackSprite;

    [SerializeField] Sprite individualGuardSprite;
    [SerializeField] Sprite groupGuardSprite;
    [SerializeField] Sprite allGuardSprite;

    // TODO　ガードとデバイのアイコンは再考する 

    public void SetTargetType(EquipmentType equipmentType, TargetType targetType)
    {
        if (equipmentType == EquipmentType.Weapon)
        {
            switch (targetType)
            {
                case TargetType.Self:
                    iconImage.sprite = selfSprite;
                    break;
                case TargetType.Individual:
                    iconImage.sprite = individualAttackSprite;
                    break;
                case TargetType.Group:
                    iconImage.sprite = groupAttackSprite;
                    break;
                case TargetType.All:
                    iconImage.sprite = allAttackSprite;
                    break;
                case TargetType.Random:
                    iconImage.sprite = randomSprite;
                    break;
                default:
                    Debug.LogWarning("Unknown TargetType: " + targetType);
                    break;
            }
            return;
        }
        else if (equipmentType == EquipmentType.Armor)
        {
            switch (targetType)
            {
                case TargetType.Self:
                    iconImage.sprite = selfSprite;
                    break;
                case TargetType.Individual:
                    iconImage.sprite = individualGuardSprite;
                    break;
                case TargetType.Group:
                    iconImage.sprite = groupGuardSprite;
                    break;
                case TargetType.All:
                    iconImage.sprite = allGuardSprite;
                    break;
                case TargetType.Random:
                    iconImage.sprite = randomSprite;
                    break;
                default:
                    Debug.LogWarning("Unknown TargetType: " + targetType);
                    break;
            }
            return;
        }
        else if (equipmentType == EquipmentType.Device)
        {
            switch (targetType)
            {
                case TargetType.Self:
                    iconImage.sprite = selfSprite;
                    break;
                case TargetType.Individual:
                    iconImage.sprite = individualAttackSprite;
                    break;
                case TargetType.Group:
                    iconImage.sprite = groupAttackSprite;
                    break;
                case TargetType.All:
                    iconImage.sprite = allAttackSprite;
                    break;
                case TargetType.Random:
                    iconImage.sprite = randomSprite;
                    break;
                default:
                    Debug.LogWarning("Unknown TargetType: " + targetType);
                    break;
            }
            return;
        }
    }
}
