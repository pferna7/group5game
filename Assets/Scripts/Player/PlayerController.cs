using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float maxFallSpeed = -25f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Wall Check")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckRadius = 0.2f;
    [SerializeField] private LayerMask wallLayer;

    [Header("Jump Assist")]
    [SerializeField] private float coyoteTime = 0.12f;
    [SerializeField] private float jumpBufferTime = 0.12f;

    [Header("Extra Movement Features")]
    [SerializeField] private bool enableDoubleJump = false;
    [SerializeField] private bool enableWallSlide = false;
    [SerializeField] private bool enableWallJump = false;
    [SerializeField] private int extraAirJumps = 1;

    [Header("Wall Movement")]
    [SerializeField] private float wallSlideSpeed = 2f;
    [SerializeField] private Vector2 wallJumpForce = new Vector2(10f, 14f);
    [SerializeField] private float wallJumpLockTime = 0.2f;

    [Header("Knockback")]
    [SerializeField] private float knockbackLockTime = 0.2f;

    private Rigidbody2D rb;

    private float horizontalInput;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isFacingRight = true;
    private bool controlsLocked = false;

    private float coyoteCounter;
    private float jumpBufferCounter;
    private int airJumpsRemaining;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        CheckGrounded();
        CheckWall();
        UpdateTimers();
        HandleJumpInput();
        HandleWallSlide();
        HandleFlip();
    }

    private void FixedUpdate()
    {
        if (controlsLocked)
        {
            ClampFallSpeed();
            return;
        }

        Move(horizontalInput);
        ClampFallSpeed();
    }

    private void UpdateTimers()
    {
        if (isGrounded)
        {
            coyoteCounter = coyoteTime;
            airJumpsRemaining = enableDoubleJump ? extraAirJumps : 0;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        jumpBufferCounter -= Time.deltaTime;
    }

    private void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }

        if (jumpBufferCounter > 0f)
        {
            if (enableWallJump && isWallSliding)
            {
                WallJump();
                jumpBufferCounter = 0f;
                return;
            }

            if (coyoteCounter > 0f)
            {
                Jump();
                jumpBufferCounter = 0f;
                return;
            }

            if (!isGrounded && enableDoubleJump && airJumpsRemaining > 0)
            {
                airJumpsRemaining--;
                Jump();
                jumpBufferCounter = 0f;
                return;
            }
        }

        // Variable jump height
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    public void Move(float direction)
    {
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        coyoteCounter = 0f;
    }

    private void HandleWallSlide()
    {
        bool pushingIntoWall =
            (isFacingRight && horizontalInput > 0f) ||
            (!isFacingRight && horizontalInput < 0f);

        isWallSliding = enableWallSlide && !isGrounded && isTouchingWall && pushingIntoWall;

        if (isWallSliding && rb.linearVelocity.y < -wallSlideSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
        }
    }

    private void WallJump()
    {
        int jumpDirection = isFacingRight ? -1 : 1;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(wallJumpForce.x * jumpDirection, wallJumpForce.y), ForceMode2D.Impulse);

        StartCoroutine(LockControlsTemporarily(wallJumpLockTime));
    }

    private System.Collections.IEnumerator LockControlsTemporarily(float duration)
    {
        controlsLocked = true;
        yield return new WaitForSeconds(duration);
        controlsLocked = false;
    }

    private void HandleFlip()
    {
        if (horizontalInput > 0f && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0f && isFacingRight)
        {
            Flip();
        }
    }

    public void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    public void CheckGrounded()
    {
        if (groundCheck == null)
        {
            isGrounded = false;
            return;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void CheckWall()
    {
        if (wallCheck == null)
        {
            isTouchingWall = false;
            return;
        }

        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);
    }

    public void ApplyKnockback(Vector2 force)
    {
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);

        StopAllCoroutines();
        StartCoroutine(LockControlsTemporarily(knockbackLockTime));
    }

    private void ClampFallSpeed()
    {
        if (rb.linearVelocity.y < maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxFallSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("MovingPlatform"))
        {
            transform.SetParent(collision.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("MovingPlatform"))
        {
            transform.SetParent(null);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
        }
    }
}