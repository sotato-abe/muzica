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
    private float encountChance = 0.01f; // 10% の確率
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
            OnReserveStart?.Invoke();
        }
    }

    void FixedUpdate()
    {
        Vector2 moveAmount = moveInput * moveSpeed;
        rb.MovePosition(rb.position + moveAmount * Time.fixedDeltaTime);

        if (moveInput != Vector2.zero)
        {
            CheckForEncounter();
        }
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    private void CheckForEncounter()
    {
        // EncountLayer上にいる時に確率でエンカウントを発生させる
        //　Encount時はひとまずDebug.Logで「エンカウント」と表示
        Collider2D hit = Physics2D.OverlapCircle(transform.position, encountRadius, encountLayer);
        if (hit != null)
        {
            if (Random.value < encountChance)
            {
                OnBattleStart?.Invoke();
            }
        }
    }
}
