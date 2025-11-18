using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShippingQuestTask : MonoBehaviour
{
    [Header("Task")] // タスク
    [SerializeField] Image shippingPointImage;
    [SerializeField] TextMeshProUGUI shippingPointNameTitle;
    [SerializeField] TextMeshProUGUI shippingDescriptionText;
    [SerializeField] MockItemBlock shippingItemPrefab;
    [SerializeField] GameObject shippingItemList;

    public delegate void TargetItemDelegate(Item item);
    public event TargetItemDelegate OnTargetItem;

    // ここにデリバリークエストタスクのロジックを実装
    public void SetShippingTask(DeliveryQuest quest)
    {
        if (quest == null) return;
        ClearTask();
        SetDeliveryPoint(quest.Base.PointBase);
        foreach (var item in quest.DeliveryItemList)
        {
            SetShippingItemSlot(item);
        }
    }

    private void SetDeliveryPoint(PointBase pointBase)
    {
        if (pointBase == null) return;
        var delivePoint = pointBase;
        if (delivePoint == null) return;
        shippingPointImage.sprite = delivePoint.Image;
        shippingPointNameTitle.text = delivePoint.Name;
        shippingDescriptionText.text = delivePoint.Description;
    }

    private void SetShippingItemSlot(Item item)
    {
        var slot = Instantiate(shippingItemPrefab, shippingItemList.transform);
        slot.SetMockItem(item);
        slot.OnTargetItem += TargetItem;
    }

    private void TargetItem(Item item)
    {
        OnTargetItem?.Invoke(item);
    }

    private void ClearTask()
    {
        shippingPointImage.sprite = null;
        shippingPointNameTitle.text = string.Empty;

        foreach (Transform child in shippingItemList.transform)
        {
            Destroy(child.gameObject);
        }
    }
}