using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonUX : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] public TextMeshProUGUI text;
    [SerializeField] public Image iconImage;
    [SerializeField] public Color activeColor = Color.white;
    [SerializeField] public Color inactiveColor = Color.black;

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        SoundSystem.Instance.PlaySE(SeType.Select);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        SoundSystem.Instance.PlaySE(SeType.Cursol);
        text.color = activeColor;
        iconImage.color = activeColor;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        text.color = inactiveColor;
        iconImage.color = inactiveColor;
    }
}
