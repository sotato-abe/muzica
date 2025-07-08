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

    public delegate void DropItemDelegate(ItemBlock itemBlock);
    public event DropItemDelegate OnDropItem;

    public delegate void BagInItemDelegate(ItemBlock itemBlock);
    public event BagInItemDelegate OnBagInItem;

    public delegate void EquipItemDelegate(ItemBlock itemBlock);
    public event EquipItemDelegate OnEquipItem;

    public delegate void SellItemDelegate(Item item);
    public event SellItemDelegate OnSellItem;

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

    public void DropItem()
    {
        OnDropItem?.Invoke(this);
    }

    public void BagInItem()
    {
        OnBagInItem?.Invoke(this);
    }

    public void EquipItem()
    {
        OnEquipItem?.Invoke(this);
    }

    public void SellItem()
    {
        OnSellItem?.Invoke(Item);
    }
}
