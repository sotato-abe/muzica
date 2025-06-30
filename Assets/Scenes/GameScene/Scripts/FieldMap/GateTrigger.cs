using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    private bool isTriggered = false;
    private float triggerTime = 0f;

    public Vector2Int direction = Vector2Int.zero; // 移動方向を指定するための変数

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isTriggered && other.CompareTag("Player"))
        {
            triggerTime += Time.deltaTime;

            if (triggerTime >= 2f)
            {
                isTriggered = true;
                WorldMapController.Instance.ChangePlayerCoordinate(direction);
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
