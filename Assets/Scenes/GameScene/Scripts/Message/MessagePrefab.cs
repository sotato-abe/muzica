using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessagePrefab : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] SlidePanel slidePanel;
    [SerializeField] RectTransform backRectTransform;
    [SerializeField] TextMeshProUGUI text;

    private int lineWidth = 700;
    private int minHeight = 50;
    private float padding = 0f;

    private void OnStart()
    {
        StartCoroutine(CloseMessage());
    }

    public void SetMessage(Message message)
    {
        text.SetText(message.messageText);
        ResizePlate();
        image.sprite = message.sprite;
        slidePanel.SetActive(true);
    }

    private IEnumerator CloseMessage()
    {
        yield return new WaitForSeconds(10.0f);
        slidePanel.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        gameObject.SetActive(false);
        yield return null;
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
