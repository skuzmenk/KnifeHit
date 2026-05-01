using UnityEngine;

public class TargetRotation : MonoBehaviour
{
    [Header("Speed Settings")]
    public float baseSpeed = 100f;       
    public float speedStep = 10f;        
    public float maxSpeedLimit = 400f;   

    private float currentLevelSpeed;     
    private bool isBoss = false;
    private float timer = 0;

    private SpriteRenderer sr;
    private CircleCollider2D circleCollider;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if (!isBoss)
        {
            transform.Rotate(0, 0, currentLevelSpeed * Time.deltaTime);
        }
        else
        {
            timer += Time.deltaTime;
            float activeSpeed = Mathf.Cos(timer * 0.5f) * (currentLevelSpeed * 2f);
            transform.Rotate(0, 0, activeSpeed * Time.deltaTime);
        }
    }

    public void ChangeTargetAppearance(Sprite newSprite, bool bossStatus, int level)
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        if (circleCollider == null) circleCollider = GetComponent<CircleCollider2D>();

        if (newSprite != null)
        {
            sr.sprite = newSprite;
            circleCollider.radius = newSprite.bounds.extents.x;
            AdjustScale();
        }

        isBoss = bossStatus;

        currentLevelSpeed = baseSpeed + (level * speedStep);

        if (currentLevelSpeed > maxSpeedLimit) currentLevelSpeed = maxSpeedLimit;

        Debug.Log("Рівень: " + level + " | Швидкість: " + currentLevelSpeed);
    }

    void AdjustScale()
    {
        float targetSize = 2.5f;
        float currentWidth = sr.sprite.bounds.size.x;
        float scaleFactor = targetSize / currentWidth;
        transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
    }
}