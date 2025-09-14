using UnityEngine;
using UnityEngine.UI;

public class RarityIcon : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] Sprite commonSprite;
    [SerializeField] Sprite rareSprite;
    [SerializeField] Sprite epicSprite;
    [SerializeField] Sprite legendarySprite;
    [SerializeField] Sprite mythicSprite;

    public void SetRarityIcon(RarityType type)
    {
        switch (type)
        {
            case RarityType.Common:
                iconImage.sprite = commonSprite;
                break;
            case RarityType.Rare:
                iconImage.sprite = rareSprite;
                break;
            case RarityType.Epic:
                iconImage.sprite = epicSprite;
                break;
            case RarityType.Legendary:
                iconImage.sprite = legendarySprite;
                break;
            case RarityType.Mythic:
                iconImage.sprite = mythicSprite;
                break;
            default:
                Debug.LogWarning("Unknown rarity type");
                break;
        }
    }
}