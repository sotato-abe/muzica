using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DropItem : MonoBehaviour
{
    private Item item;
    private bool canPickup = false;

    public void Setup(Item droppedItem)
    {
        item = droppedItem;

        StartCoroutine(EnablePickupAfterDelay(1f));

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 randomDir = new Vector2(
                Random.value < 0.5f ? -1f : 1f,
                Random.value < 0.5f ? -1f : 1f
            ).normalized;

            float power = Random.Range(1f, 2f);
            rb.AddForce(randomDir * power, ForceMode2D.Impulse);

            StartCoroutine(StopAfterDelay(rb, 0.4f));
        }
    }

    private IEnumerator EnablePickupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canPickup = true;
    }

    private IEnumerator StopAfterDelay(Rigidbody2D rb, float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!canPickup) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log($"Item {item.Base.Name} picked up by player.");
            PlayerController.Instance.AddItemToBag(item);
            Destroy(gameObject);
        }
    }
}
