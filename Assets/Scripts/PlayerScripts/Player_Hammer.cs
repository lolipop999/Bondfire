using UnityEngine;

public class Player_Hammer : MonoBehaviour
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
        if (Input.GetButtonDown("Attack") && timer <= 0)
        {
            Smash();
        }
    }

    public void Smash()
    {
        anim.SetBool("isSmashing", true);
        timer = StatsManager.Instance.hammerCoolDown;
    }

    public void SmashDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, StatsManager.Instance.hammerRange, enemyLayer);
        FXManager.Instance.PlaySound(FXManager.Instance.hammerAttack, 0.3f);
        if (enemies.Length > 0)
        {
            foreach (Collider2D enemy in enemies)
            {
                enemy.GetComponent<Enemy_Health>().ChangeHealth(-StatsManager.Instance.damage);
                enemy.GetComponent<Enemy_Knockback>().Knockback(transform, StatsManager.Instance.knockbackForce * 1.3f, StatsManager.Instance.knockbackTime, StatsManager.Instance.stunTime * 1.3f);
            }
        }
    }

    public void FinishSmash()
    {
        anim.SetBool("isSmashing", false);
    }

    private void OnEnable()
    {
        anim.SetLayerWeight(2, 1);
    }
    private void OnDisable()
    {
        anim.SetLayerWeight(2, 0);
    }
}
