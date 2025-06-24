using System.Collections;
using System.Numerics;
using UnityEngine;

public class Enemy_Knockback : MonoBehaviour
{
    private Rigidbody2D rb;
    private Enemy_Movement enemy_Movement;

    private void Start()
    {
        enemy_Movement = GetComponent<Enemy_Movement>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void Knockback(Transform forceTransform, float knockbackForce, float knockbackTime, float stunTime)
    {
        enemy_Movement.ChangeState(EnemyState.Knockback);
        UnityEngine.Vector2 direction = (transform.position - forceTransform.position).normalized;
        rb.linearVelocity = direction * knockbackForce;
        StartCoroutine(StunTimer(knockbackTime, stunTime));
    }

    IEnumerator StunTimer(float knockbackTime, float stunTime) {
        yield return new WaitForSeconds(knockbackTime);
        rb.linearVelocity = UnityEngine.Vector2.zero;
        yield return new WaitForSeconds(stunTime);
        enemy_Movement.ChangeState(EnemyState.Idle);
    }
}
