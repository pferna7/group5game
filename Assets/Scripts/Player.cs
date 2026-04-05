using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class Player : MonoBehaviour
{
    public int health = 100;
    public int coins = 0;

    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpForce = 10f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI coinText;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float moveInput;
    private bool isRunning;
    private string currentAnimation = "";

    public bool speedBoost;
    public float speedBoostMultiplier = 2f;
    public float speedBoostDuration = 5f;

    public bool hasDoubleJumpPwrUP = false;   // starts OFF
    private int extraJumps;
    public int extraJumpsValue = 1;             // 1 extra jump = double jump

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        extraJumps = 0; // no double jump at start
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");
        isRunning = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            else if (hasDoubleJumpPwrUP && extraJumps > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                extraJumps--;
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
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        if (speedBoost) currentSpeed *= speedBoostMultiplier;

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        // reset jumps only if player has unlocked the powerup
        if (isGrounded)
        {
            if (hasDoubleJumpPwrUP)
                extraJumps = extraJumpsValue;
            else
                extraJumps = 0;
        }

        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);
    }

    void SetAnimation()
    {
        if (!isGrounded)
        {
            if (rb.linearVelocity.y > 0.1f)
                ChangeAnimation("Jump");
            else
                ChangeAnimation("Fall");

            return;
        }

        if (Mathf.Abs(moveInput) < 0.1f)
            ChangeAnimation("Idle");
        else if (isRunning)
            ChangeAnimation("Run");
        else
            ChangeAnimation("Walking");
    }

    void ChangeAnimation(string animationName)
    {
        if (currentAnimation == animationName) return;

        animator.Play(animationName);
        currentAnimation = animationName;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            health -= 25;
            health = Mathf.Max(0, health);

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            StartCoroutine(BlinkRed());

            if (health <= 0)
            {
                Die();
            }
        }
    }

  private void OnTriggerEnter2D(Collider2D collision)
{
    Debug.Log("Touched trigger: " + collision.gameObject.name + " | Tag: " + collision.gameObject.tag);

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

IEnumerator DoubleJumpCoroutine()
{
    hasDoubleJumpPwrUP = true; 
    extraJumps = extraJumpsValue;

    yield return new WaitForSeconds(7f);

    hasDoubleJumpPwrUP = false;
    extraJumps = 0;
}
    IEnumerator SpeedBoostCoroutine()
    {
        speedBoost = true;
        yield return new WaitForSeconds(speedBoostDuration);
        speedBoost = false;
    }

    IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
}