using UnityEditor.Tilemaps;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public float speed = 3;
    public float attackRange = 2;
    public float attackCoolDown = 2;
    public float playerDetectRange = 5;
    public Transform detectionPoint;
    public LayerMask playerLayer;
    private float attackCoolDownTimer;
    private Rigidbody2D rb;
    private Transform player;
    private Animator anim;
    private int facingDirection = 1;
    private EnemyState enemyState;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ChangeState(EnemyState.Idle);
    }

    void Update()
    {
        if (enemyState != EnemyState.Knockback)
        {
            CheckForPlayer();
            if (attackCoolDownTimer > 0)
            {
                attackCoolDownTimer -= Time.deltaTime;
            }
            if (enemyState == EnemyState.Chasing)
            {
                Chase();
            }
            else if (enemyState == EnemyState.Attacking)
            {
                rb.linearVelocity = Vector2.zero;
            }        
        }
    }

        void Chase()
        {
            if (player.position.x > transform.position.x && facingDirection == -1 ||
                player.position.x < transform.position.x && facingDirection == 1)
            {
                Flip();
            }
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;
        }

        void Flip()
        {
            facingDirection *= -1;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        void CheckForPlayer()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectRange, playerLayer);

            if (hits.Length > 0)
            {
                player = hits[0].transform;

                // if player is in attack range AND cooldown is ready
                if (Vector2.Distance(transform.position, player.position) <= attackRange && attackCoolDownTimer <= 0)
                {
                    attackCoolDownTimer = attackCoolDown;
                    ChangeState(EnemyState.Attacking);
                    rb.linearVelocity = Vector2.zero;
                }
                else if (Vector2.Distance(transform.position, player.position) > attackRange && enemyState != EnemyState.Attacking)
                {
                    ChangeState(EnemyState.Chasing);
                }
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                ChangeState(EnemyState.Idle);
            }
        }

        public void ChangeState(EnemyState newState)
        {
            if (enemyState == EnemyState.Idle)
                anim.SetBool("isIdle", false);
            else if (enemyState == EnemyState.Chasing)
                anim.SetBool("isChasing", false);
            else if (enemyState == EnemyState.Attacking)
                anim.SetBool("isAttacking", false);

            enemyState = newState;

            if (enemyState == EnemyState.Idle)
                anim.SetBool("isIdle", true);
            else if (enemyState == EnemyState.Chasing)
                anim.SetBool("isChasing", true);
            else if (enemyState == EnemyState.Attacking)
                anim.SetBool("isAttacking", true);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(detectionPoint.position, playerDetectRange);
        }
    }

public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
    Knockback
}
