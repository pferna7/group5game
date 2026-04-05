using UnityEngine;

public class DoubleJumpPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        Player player = collision.GetComponent<Player>();

        if (player == null)
        {
            player = collision.GetComponentInParent<Player>();
        }

        if (player != null)
        {
            player.UnlockDoubleJump();
            Destroy(gameObject);
        }
    }
}