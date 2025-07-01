using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Title : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] float padding = 30f;
    [SerializeField] Image backImage;
    [SerializeField] RectTransform backRectTransform;

    public IEnumerator TypeTitle(string line)
    {
        if (line == "")
        {
            backImage.color = new Color(0, 0, 0, 0);
        }else
        {
            backImage.color = new Color(0, 0, 0, 1f);
        }
        title.SetText("");
        foreach (char letter in line)
        {
            title.text += letter;
            ResizePlate();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void ResizePlate()
    {
        if (title == null || backRectTransform == null)
        {
            Debug.LogError("title または backRectTransform が null");
            return;
        }

        float newWidth = title.preferredWidth + padding;
        backRectTransform.sizeDelta = new Vector2(newWidth, backRectTransform.sizeDelta.y);
    }
}
