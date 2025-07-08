using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public delegate void RemoveItemDelegate(ItemBlock itemBlock);
    public event RemoveItemDelegate OnRemoveItem;


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

    public void Setup(Item item)
    {
        Item = item;
        image.sprite = Item.Base.Sprite;
        SetTarget(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetTarget(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetTarget(false);
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

    public void RemoveItem()
    {
        OnRemoveItem?.Invoke(this);
    }
}
