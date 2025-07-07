using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetIcon : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] Sprite individualImage;
    [SerializeField] Sprite grouplImage;
    [SerializeField] Sprite allImage;

    public void SetTargetType(TargetType targetType)
    {
        switch (targetType)
        {
            case TargetType.Individual:
                iconImage.sprite = individualImage;
                break;
            case TargetType.Group:
                iconImage.sprite = grouplImage;
                break;
            case TargetType.All:
                iconImage.sprite = allImage;
                break;
            default:
                Debug.LogWarning("Unknown TargetType: " + targetType);
                break;
        }
    }
}
