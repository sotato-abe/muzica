using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FieldPlayer : FieldCharacter
{
    public UnityAction OnReserveStart; // リザーブイベント
    public UnityAction OnBattleStart; // バトルイベント
    [SerializeField] private LayerMask encountLayer;
    private float moveSpeed = 2f;
    private float encountRadius = 0.1f;
    private float encountChance = 0.05f; // 1% の確率
    private float encounterCheckInterval = 0.4f; // チェック間隔（秒）
    private float encounterCheckTimer = 0f;
    Rigidbody2D rb;
    Vector2 moveInput;

    bool canMove = true;

    private void Start()
    {
        animator = GetComponent<Animator>(); // ← 基底クラスの animator を使用
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!canMove)
        {
            moveInput = Vector2.zero; // ← 追加: 入力をリセット
            animator.SetBool("isRunning", false); // ← 走りアニメーションも止める
            return;
        }

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = moveInput.normalized;

        bool isMoving = moveInput.sqrMagnitude > 0;
        animator.SetBool("isRunning", isMoving);

        if (moveInput.x < 0)
        {
            Inversion(true); // 左向き
        }
        else if (moveInput.x > 0)
        {
            Inversion(false); // 右向き
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
            encounterCheckTimer += Time.fixedDeltaTime;
            if (encounterCheckTimer >= encounterCheckInterval)
            {
                CheckForEncounter();
                encounterCheckTimer = 0f;
            }
        }
        else
        {
            // 止まっている時はタイマーをリセットしてもいい（連続チェックを防ぐ）
            encounterCheckTimer = 0f;
        }
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
        if (!canMove)
        {
            moveInput = Vector2.zero;
            animator.SetBool("isRunning", false);
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
                animator.SetBool("isRunning", false);
                OnBattleStart?.Invoke();
            }
        }
    }
}
