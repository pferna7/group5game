using UnityEngine;

public class SaturnDamageSound : MonoBehaviour
{
    public AudioClip damageSound;
    private AudioSource audioSource;
    private Player player;
    private int lastHealth;

    void Start()
    {
        player = FindObjectOfType<Player>();
        if (player != null)
        {
            lastHealth = player.health;
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = damageSound;
            audioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        if (player == null) return;

        if (player.health < lastHealth)
        {
            audioSource.Play();
        }
        lastHealth = player.health;
    }
}