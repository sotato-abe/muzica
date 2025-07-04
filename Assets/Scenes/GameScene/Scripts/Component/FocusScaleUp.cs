using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FocusScaleUp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float scale = 1.2f;
    float time = 0.05f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(ScaleUp());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(ScaleInitialize());
    }

    private IEnumerator ScaleUp()
    {
        float currentTime = 0f;
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = new Vector3(scale, scale, scale);
        while (currentTime < time)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
    }

    private IEnumerator ScaleInitialize()
    {
        float currentTime = 0f;

        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = new Vector3(1, 1, 1);
        while (currentTime < time)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
    }
}
