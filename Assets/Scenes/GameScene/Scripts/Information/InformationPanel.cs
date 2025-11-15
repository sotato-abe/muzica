using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class InformationPanel : SlidePanel
{
    public static InformationPanel Instance { get; private set; }

    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] Image image;

    float minHeight = 140f;
    float padding = 90f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 重複防止
            return;
        }
        Instance = this;
    }

    public void SetInformation(Information information)
    {
        titleText.text = information.Base.Title;
        descriptionText.text = information.Base.Description;
        SetImage(information.Base.Sprite);
        SetPanelSize();
        StartCoroutine(DisplayInformation());
    }

    public void SetFieldInformation(FieldBase fieldBase)
    {
        titleText.text = fieldBase.FieldName;
        descriptionText.text = fieldBase.Description;
        SetImage(fieldBase.FieldSprite);
        SetPanelSize();
        StartCoroutine(DisplayInformation());
    }

    private void SetImage(Sprite sprite)
    {
        if (sprite != null)
        {
            image.gameObject.SetActive(true);
            image.sprite = sprite;
        }
        else
        {
            image.gameObject.SetActive(false);
        }
    }

    public void SetPointInformation(PointBase pointBase)
    {
        titleText.text = pointBase.Name;
        descriptionText.text = pointBase.Description;
        SetImage(pointBase.Image);
        SetPanelSize();
        StartCoroutine(DisplayInformation());
    }

    private IEnumerator DisplayInformation()
    {
        this.SetActive(true);
        yield return new WaitForSeconds(5f);
        this.SetActive(false);
    }

    private void SetPanelSize()
    {
        // descriptionText内のテキストの高さに基づいてパネルの高さを調整
        descriptionText.ForceMeshUpdate();
        float descriptionHeight = descriptionText.preferredHeight;
        float newHeight = Mathf.Max(minHeight, descriptionHeight + padding);
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newHeight);
    }
}
