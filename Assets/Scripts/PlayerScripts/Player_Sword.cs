using UnityEngine;

public class Player_Sword : MonoBehaviour
{
    public Animator anim;
    public Transform attackPoint;
    public LayerMask enemyLayer;

    private float timer;
    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public void Attack()
    {
        if (timer <= 0)
        {
            anim.SetBool("isSlashing", true);
            timer = StatsManager.Instance.swordCoolDown;
        }
    }

    public void DealDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, StatsManager.Instance.weaponRange, enemyLayer);

        if (enemies.Length > 0)
        {
            foreach (Collider2D enemy in enemies)
            {
                FXManager.Instance.PlaySound(FXManager.Instance.swordHitEnemy);
                enemy.GetComponent<Enemy_Health>().ChangeHealth(-StatsManager.Instance.damage);
                enemy.GetComponent<Enemy_Knockback>().Knockback(transform, StatsManager.Instance.knockbackForce, StatsManager.Instance.knockbackTime, StatsManager.Instance.stunTime);
            }
        }
        FXManager.Instance.PlaySound(FXManager.Instance.swordSwing);
    }

    public void FinishAttacking()
    {
        anim.SetBool("isSlashing", false);
    }

    private void OnEnable()
    {
        anim.SetLayerWeight(0, 1);
    }
    private void OnDisable()
    {
        anim.SetLayerWeight(0, 0);
    }
}
