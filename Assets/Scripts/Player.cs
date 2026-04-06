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

    [Header("Double Jump")]
    public bool enableDoubleJump = false;
    public ShowDoubleJumpMessage doubleJumpMessage;

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

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        CheckGrounded();
        UpdateHealthText();
        UpdateCoinText();
    }

    private void Update()
    {
        CheckGrounded();

        if (isGrounded)
        {
            hasDoubleJumped = false;
        }

        moveInput = Input.GetAxisRaw("Horizontal");
        isRunning = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
            }
            else if (enableDoubleJump && !hasDoubleJumped)
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

    private void FixedUpdate()
    {
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void CheckGrounded()
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

    private void SetAnimation()
    {
        if (animator == null) return;

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

    private void ChangeAnimation(string animationName)
    {
        if (animator == null) return;
        if (currentAnimation == animationName) return;

        animator.Play(animationName);
        currentAnimation = animationName;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Damage") && canTakeDamage)
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
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

    private IEnumerator DamageCooldownRoutine()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    private IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // When the player dies, reload the current scene
    }

    public void UnlockDoubleJump()
    {
        if (enableDoubleJump)
            return;

        enableDoubleJump = true;
        hasDoubleJumped = false;

        if (doubleJumpMessage != null)
        {
            doubleJumpMessage.ShowMessageNow();
        }

        Debug.Log("Double jump unlocked!");
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    private void UpdateHealthText()
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

    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = "$" + coins;
            coinText.color = Color.yellow;
        }
    }
}