using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public int health = 100;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    public TextMeshProUGUI healthText;

    private Rigidbody2D rb;
    private bool isGrounded;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // 👇 Update text + color
        if (healthText != null)
        {
            healthText.text = "♥ " + health;

            if (health <= 25)
            {
                healthText.color = Color.red;
            }
            else if (health <= 50)
            {
                healthText.color = Color.yellow;
            }
            else
            {
                healthText.color = Color.white;
            }
        }
    }

    private void FixedUpdate() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Damage") {
            health -= 25;
            health = Mathf.Max(0, health); // prevents negative

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            StartCoroutine(BlinkRed());

            if (health <= 0) {
                Die();
            }
        }
    }

    private IEnumerator BlinkRed() {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void Die() {
        SceneManager.LoadScene("GameScene");
    }
}