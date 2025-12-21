using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CommandImage : MonoBehaviour
{
    [SerializeField] Image commandImage;
    public RectTransform rectTransform;
    public Command command;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (commandImage == null)
        {
            commandImage = GetComponent<Image>();
        }
    }

    public void SetCommand(Command command)
    {
        if (command == null || command.Base == null)
        {
            commandImage.sprite = null; // もしくはデフォルトのスプライト
            commandImage.color = new Color(0, 0, 0, 0.5f); // 半透明にする
            this.command = null;
            return;
        }

        commandImage.sprite = command.Base.Sprite;
        commandImage.color = new Color(1f, 1f, 1f, 1f);
        this.command = command;
    }

    public IEnumerator ActivateCommand()
    {
        if (command == null) yield break;

        // 簡単なアニメーション例: 拡大して元に戻る
        float activateDuration = 0.07f;
        Vector3 originalScale = rectTransform.localScale;
        Vector3 targetScale = originalScale * 1.1f;
        float elapsed = 0f;
        while (elapsed < activateDuration)
        {
            rectTransform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / activateDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        rectTransform.localScale = targetScale;
        elapsed = 0f;
        while (elapsed < activateDuration)
        {
            rectTransform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / activateDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        rectTransform.localScale = originalScale;
    }
}
