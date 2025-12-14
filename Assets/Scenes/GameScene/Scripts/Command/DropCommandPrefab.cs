using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DropCommandPrefab : FieldTriggerPrefab
{
    private Command command;
    private float groundY; // 初期位置を保存
    private float duration = 0.4f;

    public void Setup(Command droppedCommand)
    {
        command = droppedCommand;
    }

    public IEnumerator JumpMoveMotion(Vector2 targetPosition)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        bool hadGravity = false;

        if (rb != null)
        {
            hadGravity = rb.simulated;
            rb.simulated = false;
        }

        float elapsedTime = 0f;
        Vector2 startPosition = transform.position;

        float jumpHeight = 0.4f; // バウンドの高さ（必要に応じて調整）
        int bounceCount = 1;     // バウンドの回数（複数回にしたい場合）

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // 水平方向はLerpで直線移動
            Vector2 flatPos = Vector2.Lerp(startPosition, targetPosition, t);

            // 垂直方向にバウンドを加える（sinカーブで上に跳ねる）
            // πで1回の山、2πで2回跳ねる
            float bounce = Mathf.Sin(t * Mathf.PI * bounceCount) * jumpHeight;

            transform.position = new Vector2(flatPos.x, flatPos.y + bounce);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        if (rb != null)
        {
            rb.simulated = hadGravity;
        }
    }

    private IEnumerator StopAfterDelay(Rigidbody2D rb, float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public override void EnterAction()
    {
        PlayerController.Instance.AddCommandToStorage(command);
        Destroy(gameObject);
    }
}
