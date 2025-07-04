using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StatusText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mainText;
    [SerializeField] TextMeshProUGUI frameText;

    public void SetText(string text)
    {
        mainText.text = text;
        frameText.text = text;
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
