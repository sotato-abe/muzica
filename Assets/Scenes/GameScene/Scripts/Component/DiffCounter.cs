using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiffCounter : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] Image iconBackImage;
    [SerializeField] Sprite lifeIcon;
    [SerializeField] Sprite batteryIcon;
    [SerializeField] Sprite soulIcon;
    [SerializeField] TextMeshProUGUI counterText;
    [SerializeField] TextMeshProUGUI counterBackText;
    [SerializeField] RectTransform rectTransform;

    Color colorWhite = Color.white;
    Color currentColor = Color.white;
    Color positiveColor = new Color(185f / 255f, 255f / 255f, 0f / 255f, 1f);
    Color negativeColor = new Color(255f / 255f, 0f / 255f, 138f / 255f, 1f);

    // 自身がgameobject.activeInHierarchyでなくなった時にDestroyする
    private void OnDisable()
    {
        Destroy(this.gameObject);
    }

    public void SetDiffCounter(EnergyType type, int value)
    {
        switch (type)
        {
            case EnergyType.Life:
                iconImage.sprite = lifeIcon;
                // iconBackImage.sprite = lifeIcon;
                break;
            case EnergyType.Battery:
                iconImage.sprite = batteryIcon;
                // iconBackImage.sprite = batteryIcon;
                break;
            case EnergyType.Soul:
                iconImage.sprite = soulIcon;
                // iconBackImage.sprite = soulIcon;
                break;
            default:
                iconImage.sprite = lifeIcon; // デフォルトはライフアイコン
                // iconBackImage.sprite = lifeIcon;
                break;
        }
        if (value > 0)
        {
            counterText.text = "+" + value.ToString();
            // counterBackText.text = "+" + value.ToString();
            // currentColor = positiveColor;
        }
        else
        {
            counterText.text = value.ToString();
            // counterBackText.text = value.ToString();
            // currentColor = negativeColor;
        }

        StartCoroutine(FadeAndMoveText());
    }

    private IEnumerator FadeAndMoveText()
    {
        float duration = 2f;
        float elapsed = 0f;
        Vector3 startPos = rectTransform.anchoredPosition;
        Vector3 endPos = startPos + new Vector3(0, 50f, 0);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            rectTransform.anchoredPosition = Vector3.Lerp(startPos, endPos, t);
            iconImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1 - t);
            counterText.color = new Color(colorWhite.r, colorWhite.g, colorWhite.b, 1 - t);

            yield return null;
        }

        Destroy(this);
    }
}
