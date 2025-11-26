using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiffCounter : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] Sprite lifeIcon;
    [SerializeField] Sprite batteryIcon;
    [SerializeField] Sprite soulIcon;
    [SerializeField] TextMeshProUGUI counterText;
    [SerializeField] RectTransform rectTransform;

    Color startColor = Color.white;

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
                break;
            case EnergyType.Battery:
                iconImage.sprite = batteryIcon;
                break;
            case EnergyType.Soul:
                iconImage.sprite = soulIcon;
                break;
            default:
                iconImage.sprite = lifeIcon; // デフォルトはライフアイコン
                break;
        }
        iconImage.color = startColor;
        counterText.text = value.ToString();

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
            iconImage.color = new Color(startColor.r, startColor.g, startColor.b, 1 - t);
            counterText.color = new Color(startColor.r, startColor.g, startColor.b, 1 - t);

            yield return null;
        }

        Destroy(this);
    }
}
