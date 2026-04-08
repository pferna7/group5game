using UnityEngine;

public class SaturnHealthBoost : MonoBehaviour
{
    void Start()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.health = 120;
        }
    }
}