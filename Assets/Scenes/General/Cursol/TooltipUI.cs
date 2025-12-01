using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;
    public RectTransform panel;

    void Awake() => Instance = this;

    void Update()
    {
        panel.position = Input.mousePosition;
    }

    public void Show(string content)
    {
        panel.gameObject.SetActive(true);
    }

    public void Hide()
    {
        panel.gameObject.SetActive(false);
    }
}
