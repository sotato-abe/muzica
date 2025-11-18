using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

// バックで使用するアイテムのクラス
// 装備、消耗品、トレジャーをすべて受け入れてバックに表示するためのクラス
public class CommandBlock : Block, IPointerEnterHandler, IPointerExitHandler
{
    public Command Command { get; set; }
    [SerializeField] Image image;
    [SerializeField] Image cursor;
    [SerializeField] StatusText statusText;
    private bool isActive = true;
    public Transform OriginalParent { get; private set; }
    public delegate bool RemoveCommandDelegate(CommandBlock commandBlock);
    public event RemoveCommandDelegate OnRemoveCommand;

    public delegate void TargetCommandDelegate(CommandBlock commandBlock);
    public event TargetCommandDelegate OnTargetCommand;


    void Start()
    {
        SetTarget(false);
    }

    protected override void Awake()
    {
        base.Awake();
    }

    public void Setup(Command command, Transform originalParent)
    {
        Command = command;
        image.sprite = Command.Base.Sprite;
        this.OriginalParent = originalParent;
        SetTarget(false);
        SetStatusText();
    }

    public void SetStatusText()
    {
        if (Command.isNew)
        {
            statusText.SetText("New");
        }
        else
        {
            statusText.SetText(null);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        SetTarget(true);
        if (Command.isNew)
        {
            Command.isNew = false;
            statusText.SetText(null);
        }
        OnTargetCommand?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetTarget(false);
        OnTargetCommand?.Invoke(null);
    }

    public void SetTarget(bool activeFlg)
    {
        if (isActive == activeFlg) return;
        SoundSystem.Instance.PlaySE(SeType.Cursol);
        statusText.SetActive(false);
        isActive = activeFlg;
        Color bgColor = cursor.color;
        bgColor.a = isActive ? 1f : 0f;
        cursor.color = bgColor;
    }

    public bool RemoveCommand()
    {
        return OnRemoveCommand?.Invoke(this) ?? false;
    }
}
