using UnityEngine;

public class Player_Sword : MonoBehaviour
{
    public Animator anim;
    public Transform attackPoint;
    public LayerMask enemyLayer;
    private PlayerMovement playerMovement;
    private float timer;

    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }
    private void Update()
    {
        if (!playerMovement.isActive) return;
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Slash") && timer <= 0)
        {
            Attack();
        }
    }

    public void Attack()
    {
        anim.SetBool("isSlashing", true);
        timer = StatsManager.Instance.swordCoolDown;
    }

    public void DealDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, StatsManager.Instance.hammerRange, enemyLayer);

        if (enemies.Length > 0)
        {
            FXManager.Instance.PlaySound(FXManager.Instance.swordHitEnemy, 0.3f);
            foreach (Collider2D enemy in enemies)
            {
                if (enemy != null)
                {
                    enemy.GetComponent<Enemy_Health>().ChangeHealth(-StatsManager.Instance.damage);
                enemy.GetComponent<Enemy_Knockback>().Knockback(transform, StatsManager.Instance.knockbackForce, StatsManager.Instance.knockbackTime, StatsManager.Instance.stunTime);
                }
            }
        }
        else
        {
            FXManager.Instance.PlaySound(FXManager.Instance.swordSwing, 0.3f);
        }
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
