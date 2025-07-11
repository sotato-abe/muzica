using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlidePanel : MonoBehaviour
{
    public RectTransform rectTransform;
    public Vector3 activePosition = new Vector3(0, 0, 0);
    public Vector3 inactivePosition = new Vector3(0, 0, 0);
    public bool isActive = false;
    private Coroutine slideCoroutine;

    public virtual void SetActive(bool activeFlg, Action onComplete = null)
    {
        if (isActive == activeFlg)
        {
            onComplete?.Invoke();
            return;
        }

        isActive = activeFlg;
        // スライド中のときはその処理を中断して新しいスライドを開始
        if (slideCoroutine != null)
        {
            StopCoroutine(slideCoroutine);
        }

        Vector3 targetPos = isActive ? activePosition : inactivePosition;
        slideCoroutine = StartCoroutine(Slide(targetPos, onComplete));
    }

    private IEnumerator Slide(Vector3 targetPosition, Action onComplete)
    {
        Vector3 startPosition = rectTransform.anchoredPosition;
        float elapsedTime = 0f;
        float duration = 0.2f;

        while (elapsedTime < duration)
        {
            rectTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
        onComplete?.Invoke();
        slideCoroutine = null;
    }


}