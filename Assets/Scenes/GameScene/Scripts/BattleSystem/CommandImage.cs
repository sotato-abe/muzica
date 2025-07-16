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
}
