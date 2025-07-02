using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessagePrefab : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] RectTransform backRectTransform;
    [SerializeField] TextMeshProUGUI text;

    private int lineWidth = 700;
    private int minHeight = 50;
    private float padding = 0f;

    public void SetMessage(Message message)
    {
        text.SetText(message.messageText);
        ResizePlate();
        image.sprite = message.sprite;
    }

    private void ResizePlate()
    {
        if (text == null || backRectTransform == null)
        {
            Debug.LogError("description または backRectTransform が null");
            return;
        }

        float contentHeight = text.preferredHeight + padding;
        float newHeight = Mathf.Max(minHeight, contentHeight);
        backRectTransform.sizeDelta = new Vector2(lineWidth, newHeight);
    }
}
