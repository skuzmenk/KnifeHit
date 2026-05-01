using UnityEngine;

public class Knife : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 15f;
    private bool isThrown = false;
    private bool isStuck = false;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isThrown && !isStuck)
        {
            ThrowKnife();
        }
    }

    void ThrowKnife()
    {
        isThrown = true;
        rb.linearVelocity = Vector2.up * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isStuck) return;

        if (collision.gameObject.CompareTag("Knife"))
        {
            FindObjectOfType<GameManager>().GameOver();
            return;
        }
        if (collision.gameObject.CompareTag("Target"))
        {
            StickToTarget(collision);
        }
    }

    void StickToTarget(Collision2D collision)
    {
        isStuck = true;
        rb.linearVelocity = Vector2.zero;

        rb.angularVelocity = 0;

        rb.isKinematic = true;
        transform.SetParent(collision.transform);
        gameObject.tag = "Knife";
        FindObjectOfType<GameManager>().OnHit();
    }

}