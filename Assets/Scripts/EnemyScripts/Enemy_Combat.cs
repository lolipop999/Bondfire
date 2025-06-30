using System.Data.Common;
using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    public EnemyData data;
    public LayerMask playerLayer;
    public Transform attackPoint;
    public void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, data.attackRange, playerLayer);
        if (hits.Length > 0)
        {
            FXManager.Instance.PlaySound(FXManager.Instance.enemyHitPlayer);
            hits[0].GetComponent<PlayerHealth>().ChangeHealth(-data.damage);
            hits[0].GetComponent<PlayerMovement>().Knockback(transform, data.knockbackForce, data.stunTime);
        }
    }
    private void OnDrawGizmosSelected() // draws circle of attack point
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, data.attackRange);
    }
    
}
