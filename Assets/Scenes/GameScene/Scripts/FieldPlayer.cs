using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FieldPlayer : MonoBehaviour
{
    public UnityAction OnReserveStart; // リザーブイベント
    public UnityAction OnBattleStart; // バトルイベント
    [SerializeField] private LayerMask encountLayer;
    Animator playerAnimator;
    private float moveSpeed = 2f;
    private float encountRadius = 0.1f;
    private float encountChance = 0.01f; // 1% の確率
    Rigidbody2D rb;
    Vector2 moveInput;

    bool canMove = true;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!canMove)
        {
            moveInput = Vector2.zero; // ← 追加: 入力をリセット
            playerAnimator.SetBool("isRunning", false); // ← 走りアニメーションも止める
            return;
        }

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = moveInput.normalized;

        bool isMoving = moveInput.sqrMagnitude > 0;
        playerAnimator.SetBool("isRunning", isMoving);

        if (moveInput.x != 0)
        {
            Vector3 currentScale = transform.localScale;
            currentScale.x = Mathf.Abs(currentScale.x) * Mathf.Sign(moveInput.x);
            transform.localScale = currentScale;
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
        {
            canMove = false;
            moveInput = Vector2.zero; // ← ここでも念のためリセット
            OnReserveStart?.Invoke();
        }
    }

    void FixedUpdate()
    {
        Vector2 moveAmount = moveInput * moveSpeed;
        rb.MovePosition(rb.position + moveAmount * Time.fixedDeltaTime);

        if (moveInput != Vector2.zero && canMove)
        {
            CheckForEncounter();
        }
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
        if (!canMove)
        {
            moveInput = Vector2.zero;
            playerAnimator.SetBool("isRunning", false);
        }
    }

    private void CheckForEncounter()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, encountRadius, encountLayer);
        if (hit != null)
        {
            if (Random.value < encountChance)
            {
                SetCanMove(false); // プレイヤーの移動を停止
                playerAnimator.SetBool("isRunning", false);
                OnBattleStart?.Invoke();
            }
        }
    }
}
