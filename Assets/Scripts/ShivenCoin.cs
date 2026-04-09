using UnityEngine;

public class ShivenCoin : MonoBehaviour
{


private void OnTriggerEnter2D(Collider2D collision) {

    if (collision.gameObject.tag == "Player") {

        ShivenPlayer player = collision.gameObject.GetComponent<ShivenPlayer>();
        player.coins += 1;
        Destroy(gameObject);
        Debug.Log("Added coin total: " + player.coins);
    }

}

}
