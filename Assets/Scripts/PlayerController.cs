using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LayerMask whatIsGround;
    public ScoreManager scoreManager;
    public CarrotManager carrotManager;
    public ParticleSystem deathParticles;
    public ParticleSystem deathParticles2;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform overlapBoxTransform;

    private float jumpForce = 15;
    private float jumpForceMin = 4;

    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        overlapBoxTransform = transform.Find("BoxOverlap");
    }

    private void Update()
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

    private void Die()
    {
        scoreManager.StopScoring();

        deathParticles.transform.parent = null;
        deathParticles.Play();
        deathParticles2.transform.parent = null;
        deathParticles2.Play();
        Destroy(deathParticles, 6);

        // Stops all platform movement
        Rigidbody2D[] allRB = FindObjectsOfType<Rigidbody2D>();
        foreach(Rigidbody2D rigidBody in allRB)
        {
            rigidBody.velocity = new Vector2(0, 0);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the player collides with an enemy
        if(collision.gameObject.layer == 9)
        {
            Die();
        }
        // If the player collides with a corrupt carrot, destroy it and then die.
        else if(collision.gameObject.layer == 10)
        {
            Destroy(collision.gameObject);
            Die();
        }
        // If the player collides with a carrot
        else if(collision.gameObject.layer == 12)
        {
            carrotManager.AcquireCarrot();
            Destroy(collision.gameObject);
            // TODO: Collecting carrots
        }
        // If the player collides with a golden carrot
        else if(collision.gameObject.layer == 13)
        {
            carrotManager.AcquireGoldenCarrot();
            Destroy(collision.gameObject);
        }
    }
}
