using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    public FieldData nextField;
    private bool isTriggered = false;
    private float triggerTime = 0f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isTriggered && other.CompareTag("Player"))
        {
            triggerTime += Time.deltaTime;

            if (triggerTime >= 1f)
            {
                isTriggered = true;
                FieldTransitionManager.Instance.LoadField();
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
