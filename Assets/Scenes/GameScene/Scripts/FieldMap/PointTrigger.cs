using UnityEngine;

public class PointTrigger : MonoBehaviour
{
    private bool isTriggered = false;
    private float triggerTime = 0f;

    public PointBase point;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isTriggered && other.CompareTag("Player"))
        {
            triggerTime += Time.deltaTime;

            if (triggerTime >= 0.5f)
            {
                FieldController.Instance.EnterPoint(point);
                isTriggered = true; // イベント発動後、フラグを立てる
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
}
