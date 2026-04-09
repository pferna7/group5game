using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        NaufilPlayer player = collision.GetComponent<NaufilPlayer>();

        if (player == null)
        {
            player = collision.GetComponentInParent<NaufilPlayer>();
        }

        if (player != null)
        {
            player.AddCoins(1);
            Destroy(gameObject);
            Debug.Log("Added coin total: " + player.coins);
        }
    }
}