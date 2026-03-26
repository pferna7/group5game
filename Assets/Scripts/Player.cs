using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player : MonoBehaviour
{
    public int health = 100;

    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpForce = 10f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float moveInput;
    private bool isRunning;
    private string currentAnimation = "";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        isRunning = Input.GetKey(KeyCode.R);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (moveInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
        }

        SetAnimation();
    }

    void FixedUpdate()
    {
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        rb.velocity = new Vector2(moveInput * currentSpeed, rb.velocity.y);

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    void SetAnimation()
    {
        if (!isGrounded)
        {
            if (rb.velocity.y > 0.1f)
            {
                ChangeAnimation("Jump");
            }
            else
            {
                ChangeAnimation("Fall");
            }
            return;
        }

        if (Mathf.Abs(moveInput) < 0.1f)
        {
            ChangeAnimation("Idle");
        }
        else if (isRunning)
        {
            ChangeAnimation("Run");
        }
        else
        {
            ChangeAnimation("Walking");
        }
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
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            StartCoroutine(BlinkRed());

            if (health <= 0)
            {
                Die();
            }
        }
    }

    IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    void Die()
    {
        SceneManager.LoadScene("GameScene");
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}