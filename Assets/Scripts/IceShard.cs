using UnityEngine;
using UnityEngine.SceneManagement;

public class IceShard : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 5f;
    [SerializeField] private int damage = 50;

    void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.health -= damage;
                player.health = Mathf.Max(0, player.health);
                if (player.health <= 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // When the player dies, reload the current scene
                }
            }
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}