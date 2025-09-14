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
        title.SetText("");
        foreach (char letter in line)
        {
            title.text += letter;
            ResizePlate();
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void ChangeTitle(string newTitle)
    {
        StopAllCoroutines();
        StartCoroutine(TypeTitle(newTitle));
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
