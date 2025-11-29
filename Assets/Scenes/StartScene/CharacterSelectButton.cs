using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CharacterSelectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image characterImage;
    [SerializeField] private Image frameImage;
    [SerializeField] RectTransform buttonTransform;

    public delegate void SelectCharacterDelegate(int index);
    public event SelectCharacterDelegate OnSelectCharacter;

    public int characterIndex;

    Color activeColor = new Color(196f / 255f, 255f / 255f, 0 / 255f, 1f);
    Color inactiveColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 200f / 255f);

    int width = 100;
    int defaultHeight = 50;

    float resizeDuration = 0.1f;

    private void Awake()
    {
        frameImage.gameObject.SetActive(false);
    }

    public void SetCharacter(int index, Sprite characterSprite)
    {
        characterIndex = index;
        characterImage.sprite = characterSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // frameImage.color = activeColor;
        OnSelectCharacter?.Invoke(characterIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // frameImage.color = inactiveColor;
    }

    public void SelectButton(bool isSelected)
    {
        if (isSelected)
            SoundSystem.Instance.PlaySE(SeType.Cursol);
        SetActiveButton(isSelected);
    }

    public void SetActiveButton(bool isSelected)
    {
        if (isSelected)
        {
            frameImage.gameObject.SetActive(true);
            StartCoroutine(SmoothResize(buttonTransform, new Vector2(width, width)));
        }
        else
        {
            frameImage.gameObject.SetActive(false);
            StartCoroutine(SmoothResize(buttonTransform, new Vector2(width, defaultHeight)));
        }
    }

    // サイズ変更をスムーズに行うコルーチン
    private IEnumerator SmoothResize(RectTransform rectTransform, Vector2 targetSize)
    {
        Vector2 initialSize = rectTransform.sizeDelta;
        float elapsedTime = 0f;

        while (elapsedTime < resizeDuration)
        {
            rectTransform.sizeDelta = Vector2.Lerp(initialSize, targetSize, (elapsedTime / resizeDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.sizeDelta = targetSize;
    }
}