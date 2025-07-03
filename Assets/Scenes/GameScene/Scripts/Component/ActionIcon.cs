using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ActionIcon : MonoBehaviour
{
    [SerializeField] Image backImage;
    [SerializeField] private bool isActive = false;
    [SerializeField] Color defaultColor = new Color(0, 0, 0, 200);
    [SerializeField] Color activeColor = new Color(133, 10, 255, 200);
    private float defaultSize = 50f;
    private float activeScale = 1.3f;
    private float scaleDuration = 0.05f;
    private RectTransform rectTransform;
    // Color32 defaultColor = new Color32(0, 0, 0, 200);

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetActive(bool activeFlg)
    {
        if (isActive == activeFlg) return;
        isActive = activeFlg;
        StopAllCoroutines();
        SetColor(isActive);
        float targetSize = isActive ? defaultSize * activeScale : defaultSize;
        StartCoroutine(ResizeOverTime(targetSize));
    }

    private IEnumerator ResizeOverTime(float targetSize)
    {
        float elapsedTime = 0f;
        Vector2 startSize = rectTransform.sizeDelta;
        Vector2 endSize = new Vector2(targetSize, targetSize);

        // var layout = GetComponent<LayoutElement>();

        while (elapsedTime < scaleDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / scaleDuration;

            Vector2 currentSize = Vector2.Lerp(startSize, endSize, t);
            rectTransform.sizeDelta = currentSize;
            // layout.preferredHeight = currentSize.y;

            yield return null;
        }

        rectTransform.sizeDelta = endSize;
        // layout.preferredHeight = targetSize;
    }

    private void SetColor(bool isActive)
    {
        if (isActive)
            backImage.color = activeColor;
        else
            backImage.color = defaultColor;
    }
}
