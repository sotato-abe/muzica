using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SystemPanel : MonoBehaviour
{
    [SerializeField] Image backPanel;

    private Color activeBackPanelColor = new Color(200f / 255f, 0f / 255f, 104f / 255f, 250f / 255f);
    private Color inactiveBackPanelColor = new Color(0f, 0f, 0f, 0f);

    public virtual void PanelOpen()
    {
        StartCoroutine(BlockChange(true));
    }

    public virtual void PanelClose()
    {
        StartCoroutine(BlockChange(false));
    }

    private IEnumerator BlockChange(bool isOpen)
    {
        float duration = 0.2f;
        float elapsedTime = 0f;
        Color startColor = backPanel.color;
        Color targetColor = isOpen ? activeBackPanelColor : inactiveBackPanelColor;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            backPanel.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }
        backPanel.color = targetColor;
        backPanel.raycastTarget = isOpen;
    }
}
