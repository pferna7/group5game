using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour
{
    public float momentBeforeDip = 1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(Fall());
        }
    }

   private IEnumerator Fall()
{
    yield return new WaitForSeconds(momentBeforeDip);
    
    // Get existing Rigidbody2D, or add one if it doesn't exist
    Rigidbody2D rb = GetComponent<Rigidbody2D>();
    if (rb == null)
        rb = gameObject.AddComponent<Rigidbody2D>();

    rb.bodyType = RigidbodyType2D.Dynamic;
}
}