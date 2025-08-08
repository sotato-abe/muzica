using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

// バックで使用するアイテムのクラス
// 装備、消耗品、トレジャーをすべて受け入れてバックに表示するためのクラス
public class ItemBlock : Block, IPointerEnterHandler, IPointerExitHandler
{
    public Item Item { get; set; }
    [SerializeField] Image image;
    [SerializeField] Image cursor;
    [SerializeField] StatusText statusText;
    private bool isActive = true;
    public bool isOwned = false;
    public Transform OriginalParent { get; private set; }
    public delegate bool RemoveItemDelegate(ItemBlock itemBlock);
    public event RemoveItemDelegate OnRemoveItem;

    public delegate void TargetItemDelegate(ItemBlock itemBlock);
    public event TargetItemDelegate OnTargetItem;


    void Start()
    {
        SetTarget(false);
    }

    public void SetStatustext(string text)
    {
        statusText.SetText(text);
        statusText.SetActive(true);
    }

    protected override void Awake()
    {
        base.Awake();
    }

    public void Setup(Item item, Transform originalParent)
    {
        Item = item;
        image.sprite = Item.Base.Sprite;
        this.OriginalParent = originalParent;
        SetTarget(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        SetTarget(true);
        OnTargetItem?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetTarget(false);
        OnTargetItem?.Invoke(null);
    }

    public void SetTarget(bool activeFlg)
    {
        if (isActive == activeFlg) return;
        statusText.SetActive(false);
        isActive = activeFlg;
        Color bgColor = cursor.color;
        bgColor.a = isActive ? 1f : 0f;
        cursor.color = bgColor;
    }

    public bool RemoveItem()
    {
        return OnRemoveItem?.Invoke(this) ?? false;
    }
}
