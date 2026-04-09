using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class NaufilPlayer : MonoBehaviour
{
    public int health = 100;
    public int coins = 0;

    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpForce = 10f;
    public float bouncePadMultiplier = 2f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI coinText;

    [Header("Animation Names")]
    public string idleAnimationName = "NaufIdle";
    public string walkingAnimationName = "NaufWalking";
    public string runAnimationName = "NaufRun";
    public string jumpAnimationName = "NaufJump";
    public string fallAnimationName = "NaufFall";

    [Header("Double Jump")]
    public bool enableDoubleJump = false; // permanent unlock if needed
    public float doubleJumpPowerupDuration = 7f;
    public ShowDoubleJumpMessage doubleJumpMessage;

    [Header("Speed Boost")]
    public bool speedBoost = false;
    public float speedBoostMultiplier = 2f;
    public float speedBoostDuration = 5f;

    [Header("Damage")]
    public int damageAmount = 25;
    public float damageCooldown = 0.5f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float moveInput;
    private bool isRunning;
    private string currentAnimation = "";

    private bool hasDoubleJumped = false;
    private bool canTakeDamage = true;
    private bool doubleJumpPowerupActive = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        UpdateHealthText();
        UpdateCoinText();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        isRunning = Input.GetKey(KeyCode.LeftShift);

        if (isGrounded)
        {
            hasDoubleJumped = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
            }
            else if (CanUseDoubleJump() && !hasDoubleJumped)
            {
                Jump();
                hasDoubleJumped = true;
            }
        }

        UpdateHealthText();
        UpdateCoinText();

        if (moveInput > 0)
            spriteRenderer.flipX = false;
        else if (moveInput < 0)
            spriteRenderer.flipX = true;

        SetAnimation();
    }

    void FixedUpdate()
    {
        CheckGrounded();

        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        if (speedBoost)
            currentSpeed *= speedBoostMultiplier;

        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);
    }

    bool CanUseDoubleJump()
    {
        return enableDoubleJump || doubleJumpPowerupActive;
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    void CheckGrounded()
    {
        if (groundCheck == null)
        {
            isGrounded = false;
            return;
        }

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    void SetAnimation()
    {
        if (animator == null) return;

        if (!isGrounded)
        {
            if (rb.linearVelocity.y > 0.1f)
                ChangeAnimation(jumpAnimationName);
            else
                ChangeAnimation(fallAnimationName);
            return;
        }

        float xSpeed = Mathf.Abs(rb.linearVelocity.x);

        if (xSpeed < 0.1f)
            ChangeAnimation(idleAnimationName);
        else if (isRunning)
            ChangeAnimation(runAnimationName);
        else
            ChangeAnimation(walkingAnimationName);
    }

    void ChangeAnimation(string animationName)
    {
        if (animator == null) return;
        if (currentAnimation == animationName) return;

        animator.Play(animationName);
        currentAnimation = animationName;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        HandleHazardTrigger(collision);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        HandleHazardCollision(collision);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Touched trigger: " + collision.gameObject.name + " | Tag: " + collision.gameObject.tag);

        HandleBounceTrigger(collision);

        if (collision.CompareTag("SpeedBoostPwrUP"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(SpeedBoostCoroutine());
        }

        if (collision.CompareTag("DoubleJumpPwrUP"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(DoubleJumpCoroutine());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleBounceCollision(collision);
    }

    void HandleHazardTrigger(Collider2D collision)
    {
        if (collision.CompareTag("Damage") && canTakeDamage)
        {
            TakeDamage();
        }
    }

    void HandleHazardCollision(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage") && canTakeDamage)
        {
            TakeDamage();
        }
    }

    void HandleBounceTrigger(Collider2D collision)
    {
        if (collision.CompareTag("BouncePad"))
        {
            Bounce();
        }
    }

    void HandleBounceCollision(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BouncePad"))
        {
            Bounce();
        }
    }

    void Bounce()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * bouncePadMultiplier);
    }

    void TakeDamage()
    {
        health -= damageAmount;
        health = Mathf.Max(0, health);

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        StartCoroutine(BlinkRed());
        StartCoroutine(DamageCooldownRoutine());

        if (health <= 0)
        {
            Die();
        }
    }

    IEnumerator DamageCooldownRoutine()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    public IEnumerator DoubleJumpCoroutine()
    {
        doubleJumpPowerupActive = true;
        hasDoubleJumped = false;

        yield return new WaitForSeconds(doubleJumpPowerupDuration);

        doubleJumpPowerupActive = false;
    }

    IEnumerator SpeedBoostCoroutine()
    {
        speedBoost = true;
        yield return new WaitForSeconds(speedBoostDuration);
        speedBoost = false;
    }

    public IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UnlockDoubleJump()
    {
        if (enableDoubleJump) return;

        enableDoubleJump = true;
        hasDoubleJumped = false;

        if (doubleJumpMessage != null)
        {
            doubleJumpMessage.ShowMessageNow();
        }

        Debug.Log("Double jump unlocked!");
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "♥ " + health;

            if (health <= 25)
                healthText.color = Color.red;
            else if (health <= 50)
                healthText.color = Color.yellow;
            else
                healthText.color = Color.white;
        }
    }

    void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = "$" + coins;
            coinText.color = Color.yellow;
        }
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateCoinText();
    }
}