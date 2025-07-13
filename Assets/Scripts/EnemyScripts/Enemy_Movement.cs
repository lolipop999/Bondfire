using System.Collections;
using System.Data.Common;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public EnemyData data;
    public LayerMask playerLayer;
    public Transform detectionPoint;
    public bool stealth;
    private float attackCoolDownTimer;
    private Rigidbody2D rb;
    private Transform player;
    private Animator anim;
    private int facingDirection = 1;
    private EnemyState enemyState;
    private float idleLow = 1f;
    private bool inForcedChase = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ChangeState(EnemyState.Idle);
        attackCoolDownTimer = data.attackCoolDown;
        stealth = StealthManager.IsStealthActive;
    }

    void Update()
    {
        if (stealth)
        {
            rb.linearVelocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
            return;
        }
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

    void OnEnable()
    {
        StealthManager.OnStealthChanged += HandleStealthChange;
    }

    void OnDisable()
    {
        StealthManager.OnStealthChanged -= HandleStealthChange;
    }

    private void HandleStealthChange(bool active)
    {
        if (active)
        {
            stealth = true;
        }
        else
        {
            stealth = false;
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
        rb.linearVelocity = direction * data.speed;
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    void CheckForPlayer()
    {
        if (enemyState == EnemyState.Knockback) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, data.playerDetectRange, playerLayer);
        if (hits.Length > 0)
        {
            inForcedChase = false;
            player = hits[0].transform;

            float distance = Vector2.Distance(transform.position, player.position);

            if (distance <= data.attackRange && attackCoolDownTimer <= 0)
            {
                attackCoolDownTimer = data.attackCoolDown;
                ChangeState(EnemyState.Attacking);
                rb.linearVelocity = Vector2.zero;
            }
            else if (enemyState != EnemyState.Attacking)
            {
                ChangeState(EnemyState.Chasing);
            }
        }
        else if (inForcedChase)
        {
            ChangeState(EnemyState.Chasing);
            player = GameObject.FindWithTag("Player").transform;
            Chase();
        }
        else
        {
            // Player not detected — start idle, then forced chase
            StartCoroutine(IdleThenBlindChase());
        }
    }

    IEnumerator IdleThenBlindChase()
    {
        rb.linearVelocity = Vector2.zero;
        ChangeState(EnemyState.Idle);

        float waitTime = Random.Range(idleLow, data.idleTime);
        yield return new WaitForSeconds(waitTime);

        inForcedChase = true;
        ChangeState(EnemyState.Chasing);
        yield return new WaitForSeconds(data.forcedChaseDuration);

        inForcedChase = false;
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
        Gizmos.DrawWireSphere(detectionPoint.position, data.playerDetectRange);
    }
}

public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
    Knockback
}
