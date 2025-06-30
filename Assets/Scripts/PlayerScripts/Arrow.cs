using System.Numerics;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // BUG: 6/16/25, when enemy hits player, player cannot move or shoot and doesn't get knockback, is just stuck
    // solved 6/20, needed to make an unsync layer mask in animator

    // BUG: 6/20/25, enemy cannot move in direction arrow hit them
    // solved
    public Rigidbody2D rb;
    public UnityEngine.Vector2 direction = UnityEngine.Vector2.right;

    public float speed;
    public int damage;
    public float knockbackForce;
    public float knockbackTime;
    public float stunTime;
    public float maxTravelDistance;

    public LayerMask enemyLayer;
    public LayerMask obstacleLayer;
    public SpriteRenderer sr;
    public Sprite buriedArrow;

    private SpriteRenderer playerRenderer;
    private float lifeSpan = 5;
    private UnityEngine.Vector2 startPosition;
    private float slowdownRate = 1f; // stimulate drag
    private bool inAir = true;
    private bool falling;
    private float rotationSpeed = 230f;
    private Collider2D arrowCollider;

    void Start()
    {
        playerRenderer = GetComponent<SpriteRenderer>();
        sr.sortingOrder = playerRenderer.sortingOrder;
        rb.linearVelocity = direction * speed;
        startPosition = transform.position;
        arrowCollider = GetComponent<Collider2D>();
        RotateArrow();
        FXManager.Instance.PlaySound(FXManager.Instance.arrowRelease, 0.15f); // sound effect
        Destroy(gameObject, lifeSpan);
    }

    void FixedUpdate()
    {
        float distanceTraveled = UnityEngine.Vector2.Distance(startPosition, transform.position);

        if (distanceTraveled >= maxTravelDistance)
        {
            falling = true;
        }
        if (falling)
        {
            float currentAngle = Mathf.Atan2(rb.linearVelocityY, rb.linearVelocityX) * Mathf.Rad2Deg;
            float targetAngle = -90f; // pointing down
            float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.fixedDeltaTime);
            transform.eulerAngles = new UnityEngine.Vector3(0, 0, newAngle);
            slowdownRate = 0.99f;
            if (distanceTraveled >= maxTravelDistance * 1.5)
            {
                // BUG: when hits ground arrow always looks exactly like sprite
                rb.linearVelocity = UnityEngine.Vector2.zero;
                sr.sprite = buriedArrow;
                inAir = false;
                arrowCollider.enabled = false;
            }
        }

        rb.linearVelocity *= slowdownRate; // drag
    }

    private void RotateArrow()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = UnityEngine.Quaternion.Euler(new UnityEngine.Vector3(0, 0, angle));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (inAir)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Collider2D playerCollider = collision.collider; // player collider

                Physics2D.IgnoreCollision(arrowCollider, playerCollider, true); // stops arrow from hitting player
            }
            if ((enemyLayer.value & (1 << collision.gameObject.layer)) > 0)
            {
                collision.gameObject.GetComponent<Enemy_Health>().ChangeHealth(-damage);
                collision.gameObject.GetComponent<Enemy_Knockback>().Knockback(transform, knockbackForce, knockbackTime, stunTime);
                AttachToTarget(collision.gameObject.transform);
                GetComponent<Collider2D>().enabled = false;
                FXManager.Instance.PlaySound(FXManager.Instance.arrowHit, 0.25f);
            }
            else if ((obstacleLayer.value & (1 << collision.gameObject.layer)) > 0)
            {
                AttachToTarget(collision.gameObject.transform);
                GetComponent<Collider2D>().enabled = false;
                FXManager.Instance.PlaySound(FXManager.Instance.arrowHit, 0.25f);
            }
        }
    }
    private void AttachToTarget(Transform target)
    {
        sr.sprite = buriedArrow;
        rb.linearVelocity = UnityEngine.Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        transform.SetParent(target);
    }
}
