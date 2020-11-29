using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public LayerMask whatIsGround;
    public ScoreManager scoreManager;
    public CarrotManager carrotManager;
    public ParticleSystem deathParticles;
    public ParticleSystem deathParticles2;
    public TerrainSpawner terrainSpawner;
    public Image blackScreen;
    public RectTransform retryButton;
    public RectTransform mainMenuButton;
    public RectTransform tweetButton;
    public Text deathScoreText;
    public RectTransform highScore;
    public ParticleSystem highScoreParticles;
    public GameObject pausePanel;
    public RectTransform pauseMenu;
    public List<Button> pauseButtons;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform overlapBoxTransform;

    private float jumpForce = 20;
    private float jumpForceMin = 6;

    private bool isGrounded;
    [HideInInspector]
    public bool canJump;
    private bool isPaused;
    private bool isClosingPause;

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
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded && !isPaused && !isClosingPause)
        {
            Jump();
        }
        else if(Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > jumpForceMin && !isPaused && !isClosingPause)
        {
            rb.velocity = new Vector2(0, jumpForceMin);
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused && !isClosingPause)
            {
                UnpauseGame();
            }
            else if(!isPaused && !isClosingPause)
            {
                SoundManager.instance.PlaySound(SoundManager.Sound.Peko4);
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

        int rand = Random.Range(0, 3);
        if(rand == 0)
        {
            SoundManager.instance.PlaySound(SoundManager.Sound.Laugh);
        }
        else if(rand == 1)
        {
            SoundManager.instance.PlaySound(SoundManager.Sound.Laugh2);
        }
        else if(rand == 2)
        {
            SoundManager.instance.PlaySound(SoundManager.Sound.Shout);
        }

        CameraShake.instance.ShakeCamera(6, 0.6f);

        deathParticles.transform.parent = null;
        deathParticles.Play();
        deathParticles2.transform.parent = null;
        deathParticles2.Play();
        Destroy(deathParticles, 6);

        // Stops all platform movement
        terrainSpawner.StopPlatforms();

        float score = scoreManager.GetScore();

        int newScore = Mathf.FloorToInt(score);

        // Saves the stats from the run
        if(newScore > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", newScore);
            StartCoroutine(HighScorePopup());
        }
        PlayerPrefs.SetInt("TotalCarrots", PlayerPrefs.GetInt("TotalCarrots") + carrotManager.GetCarrotCount());
        PlayerPrefs.SetInt("TotalRuns", PlayerPrefs.GetInt("TotalRuns", 0) + 1);
        PlayerPrefs.SetFloat("TotalDistance", PlayerPrefs.GetFloat("TotalDistance", 0) + score / 1000);

        StartCoroutine(FadeDeath());

        canJump = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        rb.gravityScale = 0;
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

        pausePanel.SetActive(true);

        // Tweens to animate the pause menu appearing
        LeanTween.scale(pauseMenu, new Vector3(0, 0, 1), 0);
        LeanTween.scale(pauseMenu, new Vector3(1.2f, 1.2f, 1), 0.1f).setIgnoreTimeScale(true).setOnComplete(RecedePauseMenu);
        void RecedePauseMenu()
        {
            LeanTween.scale(pauseMenu, new Vector3(1, 1, 1), 0.05f).setIgnoreTimeScale(true).setOnComplete(ActivateButtons);
        }
        void ActivateButtons()
        {
            foreach(Button b in pauseButtons)
            {
                b.interactable = true;
            }
        }
    }

    public void UnpauseGame()
    {
        SoundManager.instance.PlaySound(SoundManager.Sound.Click);

        isClosingPause = true;
        foreach(Button b in pauseButtons)
        {
            b.interactable = false;
        }
        LeanTween.scale(pauseMenu, new Vector3(0, 0, 1), 0.3f).setIgnoreTimeScale(true).setOnComplete(Resume);

        void Resume()
        {
            pausePanel.SetActive(false);
            isPaused = false;
            isClosingPause = false;
            scoreManager.StartScoring();
            Time.timeScale = 1;
        }
    }

    public void SetClosingPauseMenu(bool to)
    {
        isClosingPause = to;
    }

    private IEnumerator FadeDeath()
    {
        yield return new WaitForSeconds(1.5f);
        LeanTween.alpha(blackScreen.rectTransform, 1, 1).setOnComplete(DeathMenu);
        deathScoreText.text = "SCORE:\n" + Mathf.FloorToInt(scoreManager.GetScore());
        LeanTween.alphaText(deathScoreText.rectTransform, 1, 1);
    }

    private void DeathMenu()
    {
        retryButton.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);
        tweetButton.gameObject.SetActive(true);

        retryButton.GetComponent<Button>().interactable = true;
        mainMenuButton.GetComponent<Button>().interactable = true;
        tweetButton.GetComponent<Button>().interactable = true;

        LeanTween.alpha(retryButton, 1, 0.5f);
        LeanTween.alpha(mainMenuButton, 1, 0.5f);
        LeanTween.alpha(tweetButton, 1, 0.5f);
    }

    private IEnumerator HighScorePopup()
    {
        yield return new WaitForSeconds(2.5f);
        highScore.GetComponent<Image>().enabled = true;
        LeanTween.scale(highScore, new Vector3(0, 0, 1), 0);
        LeanTween.scale(highScore, new Vector3(1.6f, 1.6f, 1), .15f).setOnComplete(RecedeHighScore);
        highScoreParticles.Play();
    }

    private void RecedeHighScore()
    {
        LeanTween.scale(highScore, new Vector3(1.5f, 1.5f, 1), .05f);
    }
}
