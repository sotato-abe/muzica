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

    private int characterIndex;

    Color activeColor = new Color(196f / 255f, 255f / 255f, 0 / 255f, 1f);
    Color inactiveColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 200f / 255f);

    int width = 100;
    int defaultHeight = 60;

    private void Start()
    {
        frameImage.color = inactiveColor;
    }

    public void SetCharacter(int index, Sprite characterSprite)
    {
        characterIndex = index;
        characterImage.sprite = characterSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundSystem.Instance.PlaySE(SeType.Cursol);
        frameImage.color = activeColor;
        OnSelectCharacter?.Invoke(characterIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        frameImage.color = inactiveColor;
    }

    public void SelectButton(bool isSelected)
    {
        if (isSelected)
        {
            frameImage.color = activeColor;
            buttonTransform.sizeDelta = new Vector2(width, width);
        }
        else
        {
            frameImage.color = inactiveColor;
            buttonTransform.sizeDelta = new Vector2(width, defaultHeight);
        }
    }
}