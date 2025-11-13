using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingSystem : MonoBehaviour
{
    public static SettingSystem Instance { get; private set; }

    [SerializeField] Image backPanel;
    [SerializeField] SettingWindow settingWindow;

    private Color blockColor = new Color(0f, 0f, 0f, 200f / 255f);
    private Color inBlockColor = new Color(0f, 0f, 0f, 0f);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン切り替えても残す
        }
        else
        {
            Destroy(gameObject); // 重複防止
        }
    }

    public void OpenWindow()
    {
        settingWindow.PanelOpen();
        StartCoroutine(BlockChange(true));
    }

    public void CloseWindow()
    {
        settingWindow.ClosePanel();
        StartCoroutine(BlockChange(false));
    }

    private IEnumerator BlockChange(bool isOpen)
    {
        float duration = 0.2f;
        float elapsedTime = 0f;
        Color startColor = backPanel.color;
        Color targetColor = isOpen ? blockColor : inBlockColor;
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
