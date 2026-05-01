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

        // Клік миші (ПК)

        if (Input.GetMouseButtonDown(0) && !isThrown && !isStuck)

        {

            ThrowKnife();

        }

    }



    void ThrowKnife()

    {

        isThrown = true;



        // Запускаємо ніж вгору через фізику

        rb.linearVelocity = Vector2.up * speed;

    }



    void OnCollisionEnter2D(Collision2D collision)

    {

        // Якщо вже застряг — нічого не робимо

        if (isStuck) return;



        // Вдарився в інший ніж → програш

        if (collision.gameObject.CompareTag("Knife"))

        {

            FindObjectOfType<GameManager>().GameOver();

            return;

        }



        // Вдарився в мішень

        if (collision.gameObject.CompareTag("Target"))

        {

            StickToTarget(collision);

        }

    }



    void StickToTarget(Collision2D collision)

    {

        isStuck = true;



        // Зупиняємо фізику

        rb.linearVelocity = Vector2.zero;

        rb.angularVelocity = 0;

        rb.isKinematic = true;



        // Робимо ніж дочірнім до мішені (щоб крутився разом)

        transform.SetParent(collision.transform);



        // Тег тепер Knife (щоб інші ножі могли в нього врізатись)

        gameObject.tag = "Knife";



        // Повідомляємо GameManager

        FindObjectOfType<GameManager>().OnHit();

    }

}