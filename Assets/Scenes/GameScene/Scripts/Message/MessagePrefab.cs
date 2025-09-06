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

    public delegate bool RemoveMessageDelegate(Message message);
    public event RemoveMessageDelegate OnRemoveMessage;

    private int lineWidth = 700;
    private int minHeight = 50;
    private float padding = 0f;
    private float waitingTime = 8.0f;

    private Message currentMessage;

    public void SetMessage(Message message)
    {
        currentMessage = message;
        text.SetText(message.messageText);
        ResizePlate();
        image.sprite = message.sprite;
        slidePanel.SetActive(true);

        StartCoroutine(CloseMessage());
    }

    private IEnumerator CloseMessage()
    {
        int completed = 0;
        yield return new WaitForSeconds(waitingTime);

        void CheckAllComplete()
        {
            completed++;
            if (completed >= 1)
            {
                OnRemoveMessage?.Invoke(currentMessage);
                Destroy(gameObject);
            }
        }
        slidePanel.SetActive(false, CheckAllComplete);
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
