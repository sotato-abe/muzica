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
    [SerializeField] Image flame;
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
        SetStatusText();
        SetItemTypeFrame();
    }

    private void SetItemTypeFrame()
    {
        if (Item == null) return;
        Color frameColor = Item.Base.itemType.GetItemTypeColor();
        flame.color = frameColor;
    }

    public void SetStatusText()
    {
        if (Item.isNew)
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
        if (Item.isNew)
        {
            Item.isNew = false;
            statusText.SetText(null);
        }
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
