using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class StartButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject startIcon;

    public UnityAction OnClick;

    private void Start()
    {
        startIcon.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SoundSystem.Instance.PlaySE(SeType.Select);
        OnClick?.Invoke();
    }

    public void Setup(string storyName)
    {
        text.text = storyName;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        startIcon.SetActive(true);
        text.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        startIcon.SetActive(false);
        text.color = Color.black;
    }
}