using UnityEngine;

public class LowGravityZone : MonoBehaviour
{
    [SerializeField] private float gravityScale = 0.4f;
    [SerializeField] private float speedMultiplier = 0.6f;
    private float originalGravityScale;
    private Player playerScript;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entered low gravity zone");
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            playerScript = other.GetComponent<Player>();
            if (rb != null)
            {
                originalGravityScale = rb.gravityScale;
                rb.gravityScale = gravityScale;
            }
            if (playerScript != null)
            {
                playerScript.walkSpeed *= speedMultiplier;
                playerScript.runSpeed *= speedMultiplier;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Exited low gravity zone");
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = originalGravityScale;
            }
            if (playerScript != null)
            {
                playerScript.walkSpeed /= speedMultiplier;
                playerScript.runSpeed /= speedMultiplier;
            }
        }
    }
}