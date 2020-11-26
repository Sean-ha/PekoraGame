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
    public TerrainSpawner terrainSpawner;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform overlapBoxTransform;

    private float jumpForce = 20;
    private float jumpForceMin = 6;

    private bool isGrounded;
    [HideInInspector]
    public bool canJump;
    private bool isPaused;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        overlapBoxTransform = transform.Find("BoxOverlap");
    }

    private void Update()
    {
        CheckGrounded();
        if(canJump)
        {
            CheckInput();
        }

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
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                UnpauseGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Initiates player jump
    private void Jump()
    {
        SoundManager.instance.PlaySound(SoundManager.Sound.Jump);
        rb.velocity = new Vector2(0, jumpForce);
    }

    private void Die()
    {
        scoreManager.StopScoring();

        SoundManager.instance.PlaySound(SoundManager.Sound.Death);
        SoundManager.instance.PlaySound(SoundManager.Sound.Laugh);

        CameraShake.instance.ShakeCamera(6, 0.6f);

        deathParticles.transform.parent = null;
        deathParticles.Play();
        deathParticles2.transform.parent = null;
        deathParticles2.Play();
        Destroy(deathParticles, 6);

        // Stops all platform movement
        terrainSpawner.StopPlatforms();

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
            SoundManager.instance.PlaySound(SoundManager.Sound.CarrotPickUp);
            Die();
        }
        // If the player collides with a carrot
        else if(collision.gameObject.layer == 12)
        {
            carrotManager.AcquireCarrot();
            SoundManager.instance.PlaySound(SoundManager.Sound.CarrotPickUp);
            Destroy(collision.gameObject);
        }
        // If the player collides with a golden carrot
        else if(collision.gameObject.layer == 13)
        {
            carrotManager.AcquireGoldenCarrot();
            SoundManager.instance.PlaySound(SoundManager.Sound.GoldenCarrot);
            Destroy(collision.gameObject);
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        scoreManager.StopScoring();
        Time.timeScale = 0;
    }

    private void UnpauseGame()
    {
        isPaused = false;
        scoreManager.StartScoring();
        Time.timeScale = 1;
    }
}
