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

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        SoundSystem.Instance.PlaySE(SeType.Select);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        SoundSystem.Instance.PlaySE(SeType.Cursol);
        text.color = Color.white;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        text.color = Color.black;
    }
}
