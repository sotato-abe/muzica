using UnityEngine;

public class PointTrigger : MonoBehaviour
{
    [SerializeField] Sprite defaultSprite; // デフォルトのスプライト
    private bool isTriggered = false;
    private float triggerTime = 0f;
    public Point point;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isTriggered && other.CompareTag("Player"))
        {
            triggerTime += Time.deltaTime;

            if (triggerTime >= 0.5f)
            {
                // Null チェックを追加
                if (point != null && point.Base != null && FieldController.Instance != null)
                {
                    FieldController.Instance.EnterPoint(point);
                    isTriggered = true; // イベント発動後、フラグを立てる
                }
                else
                {
                    Debug.LogError($"PointTrigger: Null reference detected. Point: {point}, Point.Base: {point?.Base}, FieldController: {FieldController.Instance}");
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            triggerTime = 0f;
            isTriggered = false;
        }
    }

    public void SetPoint(Point newPoint)
    {
        point = newPoint;
        ChangeSpriteRenderer(); // スプライトを更新
    }

    private void ChangeSpriteRenderer()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = point?.Base?.Icon ?? defaultSprite; // Null チェックを追加
        }
    }
}
