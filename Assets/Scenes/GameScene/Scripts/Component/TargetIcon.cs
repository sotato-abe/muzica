using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetIcon : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] Sprite selfImage;
    [SerializeField] Sprite individualImage;
    [SerializeField] Sprite groupImage;
    [SerializeField] Sprite allImage;
    [SerializeField] Sprite guardImage;
    [SerializeField] Sprite randomImage;

    public void SetTargetType(TargetType targetType)
    {
        switch (targetType)
        {
            case TargetType.Self:
                iconImage.sprite = selfImage;
                break;
            case TargetType.Individual:
                iconImage.sprite = individualImage;
                break;
            case TargetType.Group:
                iconImage.sprite = groupImage;
                break;
            case TargetType.All:
                iconImage.sprite = allImage;
                break;
            case TargetType.Guard:
                iconImage.sprite = guardImage;
                break;
            case TargetType.Random:
                iconImage.sprite = randomImage;
                break;
            default:
                Debug.LogWarning("Unknown TargetType: " + targetType);
                break;
        }
    }
}
