using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldMapPanel : SlidePanel
{
    [SerializeField] WorldMapCamera worldMapCamera;
    [SerializeField] GameObject worldMap;
    [SerializeField] Title fieldTitle;
    [SerializeField] TextMeshProUGUI fieldDescription;

    float originalHeight = 550f;
    float smallHeight = 400f;
    float duration = 0.2f;

    public void SetFieldData(FieldBase fieldBase)
    {
        if (fieldBase == null || string.IsNullOrEmpty(fieldBase.FieldName))
        {
            fieldTitle.gameObject.SetActive(false);
            StartCoroutine(ResizeWorldMapPanel(false));
            return;
        }
        SetFieldName(fieldBase.FieldName);
        SetFieldDescription(fieldBase.Description);
        StartCoroutine(ResizeWorldMapPanel(true));
    }

    public void ClearFieldData()
    {
        fieldTitle.gameObject.SetActive(false);
        StartCoroutine(ResizeWorldMapPanel(false));
    }

    private void SetFieldName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            fieldTitle.gameObject.SetActive(false);
            return;
        }
        fieldTitle.gameObject.SetActive(true);
        fieldTitle.ChangeTitle(name);
    }

    private void SetFieldDescription(string description)
    {
        fieldDescription.text = description;
    }

    private IEnumerator ResizeWorldMapPanel(bool isExpanded)
    {
        // 0.5 秒かけてワールドマップパネル(worldMap)の高さを変更
        float elapsedTime = 0f;
        float startHeight = worldMap.GetComponent<RectTransform>().sizeDelta.y;
        float targetHeight = isExpanded ? smallHeight : originalHeight;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newHeight = Mathf.Lerp(startHeight, targetHeight, elapsedTime / duration);
            Vector2 sizeDelta = worldMap.GetComponent<RectTransform>().sizeDelta;
            sizeDelta.y = newHeight;
            worldMap.GetComponent<RectTransform>().sizeDelta = sizeDelta;
            yield return null;
        }

    }
}

