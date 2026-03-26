using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public Transform[] points;

    private int i;
    private SpriteRenderer spriteRenderer;
    private float fixedY;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        fixedY = transform.position.y;
    }

    void Update()
    {
        if (points == null || points.Length == 0)
            return;

        Vector3 target = new Vector3(points[i].position.x, fixedY, transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - points[i].position.x) < 0.1f)
        {
            i++;
            if (i >= points.Length)
            {
                i = 0;
            }
        }

        spriteRenderer.flipX = transform.position.x > points[i].position.x;
    }
}