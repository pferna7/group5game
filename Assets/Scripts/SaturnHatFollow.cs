using UnityEngine;

public class SaturnHatFollow : MonoBehaviour
{
    public float offsetX = 0f;     // offset when facing right
    public float offsetXFlipped = 0f; // offset when facing left
    private SpriteRenderer playerSR;

    void Start()
    {
        playerSR = transform.parent.GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        if (playerSR == null) return;

        Vector3 pos = transform.localPosition;
        pos.x = playerSR.flipX ? offsetXFlipped : offsetX;
        transform.localPosition = pos;
    }
}