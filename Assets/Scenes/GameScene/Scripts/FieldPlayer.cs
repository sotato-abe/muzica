using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fieldPlayer : MonoBehaviour
{
    Animator playerAnimator;
    private float moveSpeed = 2f;
    Rigidbody2D rb;
    Vector2 moveInput;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
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
    }

    void FixedUpdate()
    {
        Vector2 moveAmount = moveInput * moveSpeed;
        rb.MovePosition(rb.position + moveAmount * Time.fixedDeltaTime);
    }
}
