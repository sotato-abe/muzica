using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

// バックで使用するアイテムのクラス
// 装備、消耗品、トレジャーをすべて受け入れてバックに表示するためのクラス
public class QuestBlock : Block, IPointerEnterHandler, IPointerExitHandler
{
    public Quest Quest { get; set; }
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI discText;
    [SerializeField] Image backImage;
    [SerializeField] Image cursor;

    void Start()
    {
        SetTarget(false);
    }

    protected override void Awake()
    {
        base.Awake();
    }

    public void Setup(Quest quest)
    {
        Quest = quest;
        coinText.SetText("C: " + Quest.Base.CoinPrice.ToString());
        discText.SetText("D: " + Quest.Base.DiscPrice.ToString());
        SetRarityColor(Quest.Base.Rarity);
        SetTarget(false);
    }

    private void SetRarityColor(RarityType rarityType)
    {
        backImage.color = rarityType.GetRarityColor();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        // cursor
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // cursor
    }

    public void SetTarget(bool activeFlg)
    {
        Color bgColor = cursor.color;
        bgColor.a = activeFlg ? 1f : 0f;
        cursor.color = bgColor;
    }
}
