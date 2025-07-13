using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Arrow : MonoBehaviour
{
    // BUG: 6/16/25, when enemy hits player, player cannot move or shoot and doesn't get knockback, is just stuck
    // solved 6/20, needed to make an unsync layer mask in animator

    // BUG: 6/20/25, enemy cannot move in direction arrow hit them
    // solved
    public Rigidbody2D rb;
    public Vector2 direction = Vector2.right;
    public LayerMask enemyLayer;
    public LayerMask obstacleLayer;
    public SpriteRenderer sr;
    public Sprite buriedArrow;

    private SpriteRenderer playerRenderer;
    private float lifeSpan = 5;
    private Vector2 startPosition;
    private float slowdownRate = 1f; // stimulate drag
    private bool inAir = true;
    private bool falling;
    private float rotationSpeed = 230f;
    private Collider2D arrowCollider;

    void Start()
    {
        playerRenderer = GameObject.FindWithTag("Player").GetComponent<SpriteRenderer>();
        sr.sortingOrder = playerRenderer.sortingOrder - 1;
        rb.linearVelocity = direction * StatsManager.Instance.arrowSpeed;
        startPosition = transform.position;
        arrowCollider = GetComponent<Collider2D>();
        RotateArrow();
        FXManager.Instance.PlaySound(FXManager.Instance.arrowRelease, 0.15f); // sound effect
        Destroy(gameObject, lifeSpan);
    }

    void FixedUpdate()
    {
        float distanceTraveled = Vector2.Distance(startPosition, transform.position);

        if (distanceTraveled >= StatsManager.Instance.arrowMaxDistance)
        {
            falling = true;
        }
        if (falling)
        {
            float currentAngle = Mathf.Atan2(rb.linearVelocityY, rb.linearVelocityX) * Mathf.Rad2Deg;
            float targetAngle = -90f; // pointing down
            float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.fixedDeltaTime);
            transform.eulerAngles = new Vector3(0, 0, newAngle);
            slowdownRate = 0.99f;
            if (distanceTraveled >= StatsManager.Instance.arrowMaxDistance * 1.5)
            {
                // BUG: when hits ground arrow always looks exactly like sprite
                rb.linearVelocity = Vector2.zero;
                float saveAngle = transform.eulerAngles.z;
                sr.sprite = buriedArrow;
                float offset = ChooseOffset();
                transform.eulerAngles = new Vector3(0, 0, saveAngle + offset);
                inAir = false;
                arrowCollider.enabled = false;
            }
        }

        rb.linearVelocity *= slowdownRate; // drag
    }

    private float ChooseOffset()
    {
        // map each direction to an offset
        Dictionary<Vector2, float> directionOffsets = new Dictionary<Vector2, float>()
        {
            { Vector2.up, 100f },
            { Vector2.down, 280f },
            { Vector2.left, 190f },
            { Vector2.right, 0f },
            { new Vector2(1,1).normalized, 45f },
            { new Vector2(-1,1).normalized, 135f },
            { new Vector2(1,-1).normalized, -45f },
            { new Vector2(-1,-1).normalized, 225f },
        };

        float bestDot = -1f;
        float chosenOffset = 0f;


        foreach (var kvp in directionOffsets)
        {
            float dot = Vector2.Dot(direction, kvp.Key);
            if (dot > bestDot)
            {
                bestDot = dot;
                chosenOffset = kvp.Value;
            }
        }
        return chosenOffset;
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
                collision.gameObject.GetComponent<Enemy_Health>().ChangeHealth(-StatsManager.Instance.arrowDamage);
                collision.gameObject.GetComponent<Enemy_Knockback>().Knockback(transform, StatsManager.Instance.arrowKnockbackForce, StatsManager.Instance.arrowKnockbackTime, StatsManager.Instance.arrowStunTime);
                AttachToTarget(collision.gameObject.transform);
                GetComponent<Collider2D>().enabled = false;
                FXManager.Instance.PlaySound(FXManager.Instance.arrowHit, 0.25f);
            }
            else if ((obstacleLayer.value & (1 << collision.gameObject.layer)) > 0 && sr.sortingOrder < 10)
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
