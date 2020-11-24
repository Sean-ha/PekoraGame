using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LayerMask whatIsGround;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform overlapBoxTransform;

    private float jumpForce = 9;
    private float jumpForceMin = 4;

    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        overlapBoxTransform = transform.Find("BoxOverlap");
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalInput * 4, rb.velocity.y);

        CheckGrounded();
        CheckInput();

        animator.SetFloat("VerticalSpeed", rb.velocity.y);
    }

    // Checks is the player is on the ground. Called every frame.
    private void CheckGrounded()
    {
        Collider2D overlap = Physics2D.OverlapBox(overlapBoxTransform.position, new Vector2(0.45f, 0.02f), 0f,
            whatIsGround);

        if (overlap != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    
    // Checks if the player pressed Space for jumping. Called every frame.
    private void CheckInput()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
        else if(Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > jumpForceMin)
        {
            rb.velocity = new Vector2(0, jumpForceMin);
        }
    }

    // Initiates player jump
    private void Jump()
    {
        rb.velocity = new Vector2(0, jumpForce);
    }
}
