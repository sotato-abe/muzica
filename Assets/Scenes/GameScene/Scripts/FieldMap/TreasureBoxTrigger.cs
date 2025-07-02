using UnityEngine;

public class TreasureBoxTrigger : MonoBehaviour
{
    private bool isTriggered = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isTriggered && other.CompareTag("Player"))
        {
            FieldController.Instance.OpenTreasureBox();
            Destroy(gameObject);
        }
    }
}
