using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class Block : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public UnityAction OnEndDragAction;
    private CanvasGroup canvasGroup;
    private GameObject placeholder = null;
    private Transform originalParent;
    private int originalIndex;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup がアタッチされてない！");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;

        originalParent = transform.parent;
        originalIndex = transform.GetSiblingIndex();

        // プレースホルダー作成
        placeholder = new GameObject("Placeholder");
        RectTransform rt = placeholder.AddComponent<RectTransform>();
        LayoutElement le = placeholder.AddComponent<LayoutElement>();

        // サイズをコピー
        RectTransform thisRT = GetComponent<RectTransform>();
        le.preferredWidth = thisRT.rect.width;
        le.preferredHeight = thisRT.rect.height;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;

        placeholder.transform.SetParent(originalParent);
        placeholder.transform.SetSiblingIndex(originalIndex);

        // ドラッグレイヤーへ移動
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void RemovePlaceholder()
    {
        if (placeholder != null)
        {
            Destroy(placeholder);
            placeholder = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // 元の場所（プレースホルダー）に戻す
        transform.SetParent(originalParent);
        transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

        Destroy(placeholder);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("ItemUnit OnDrop");
    }

    public void Hide()
    {
        // 非表示にする
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    public void Show()
    {
        // 表示する
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}
