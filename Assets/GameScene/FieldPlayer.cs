using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fieldPlayer : MonoBehaviour
{
    Animator playerAnimator;
    private float moveSpeed = 2f;
    Vector3 currentScale;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        currentScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection += Vector3.right;
            transform.localScale = new Vector3(Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection += Vector3.left;
            transform.localScale = new Vector3(-Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveDirection += Vector3.up;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            moveDirection += Vector3.down;
        }

        if (moveDirection != Vector3.zero)
        {
            MovePlayer(moveDirection.normalized);
            playerAnimator.SetBool("isRunning", true);
        }
        else
        {
            playerAnimator.SetBool("isRunning", false);
        }
    }

    void MovePlayer(Vector3 direction)
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
