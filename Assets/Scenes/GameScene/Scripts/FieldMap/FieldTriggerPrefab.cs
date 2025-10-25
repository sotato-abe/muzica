using UnityEngine;

public class FieldTriggerPrefab : MonoBehaviour
{
    private bool isTriggered = false;
    [SerializeField] private float waitingTime = 0.5f; // トリガー発生までの待機時間
    private float triggerTime = 0f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isTriggered && other.CompareTag("Player"))
        {
            triggerTime += Time.deltaTime;

            if (triggerTime >= waitingTime)
            {
                isTriggered = true;
                EnterAction();
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

    public virtual void EnterAction()
    {

    }
}
