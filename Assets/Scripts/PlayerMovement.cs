using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D rb;
    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider;
    [SerializeField] float speed = 5;
    [SerializeField] float jumpSpeed = 5;
    [SerializeField] float climbSpeed = 5;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    private void ClimbLadder()
    {
        if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))//ko cham vao 1 layer nao do thi return (ko chay function nua)
        {
            rb.gravityScale = 8;
            myAnimator.SetBool("IsClimbing", false);
            return;
        }
        rb.velocity = new Vector2(rb.velocity.x, moveInput.y * climbSpeed);
        rb.gravityScale = 0;

        bool PlayerHasVerticalSpeed = Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("IsClimbing", PlayerHasVerticalSpeed);
    }

    private void FlipSprite()
    {
        bool PlayerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        if (PlayerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }

    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    void OnJump(InputValue value)
    {
        if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        if (value.isPressed)
        {
            rb.velocity += new Vector2(0f, jumpSpeed);
        }

    }
    void Run()
    {
        // Vector2 playerVelocity = new Vector2(moveInput.x * speed, rb.velocity.y);
        // rb.velocity = playerVelocity;
        rb.velocity = new Vector2(moveInput.x * speed, rb.velocity.y);

        bool PlayerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        myAnimator.SetBool("IsRunning", PlayerHasHorizontalSpeed);
    }
}
