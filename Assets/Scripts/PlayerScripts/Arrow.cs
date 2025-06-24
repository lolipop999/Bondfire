using UnityEngine;

public class Arrow : MonoBehaviour
{
    // BUG: 6/16/25, when enemy hits player, player cannot move or shoot and doesn't get knockback, is just stuck
    // solved 6/20, needed to make an unsync layer mask in animator

    // BUG: 6/20/25, enemy cannot move in direction arrow hit them
    public Rigidbody2D rb;
    public Vector2 direction = Vector2.right;
    public float lifeSpan = 2;
    public float speed;
    public int damage;
    public float knockbackForce;
    public float knockbackTime;
    public float stunTime;

    public LayerMask enemyLayer;
    public LayerMask obstacleLayer;
    public SpriteRenderer sr;
    public Sprite buriedArrow;

    void Start()
    {
        rb.linearVelocity = direction * speed;
        RotateArrow();
        Destroy(gameObject, lifeSpan);
    }

    private void RotateArrow()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Collider2D arrowCollider = GetComponent<Collider2D>(); // this arrow's collider
            Collider2D playerCollider = collision.collider;        // player collider

            Physics2D.IgnoreCollision(arrowCollider, playerCollider, true); // stops arrow from hitting player
        }
        if ((enemyLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            collision.gameObject.GetComponent<Enemy_Health>().ChangeHealth(-damage);
            collision.gameObject.GetComponent<Enemy_Knockback>().Knockback(transform, knockbackForce, knockbackTime, stunTime);
            AttachToTarget(collision.gameObject.transform);
            GetComponent<Collider2D>().enabled = false;
        }
        else if ((obstacleLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            AttachToTarget(collision.gameObject.transform);
            GetComponent<Collider2D>().enabled = false;
        }
    }
    private void AttachToTarget(Transform target)
    {
        sr.sprite = buriedArrow;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        transform.SetParent(target);
    }
}
