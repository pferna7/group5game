using UnityEngine;

public class NaufilCoin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                player.AddCoins(1);
                Debug.Log("Added coin total: " + player.coins);
            }

            Destroy(gameObject);
        }
    }
}