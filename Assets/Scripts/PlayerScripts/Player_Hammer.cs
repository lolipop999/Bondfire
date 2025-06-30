using UnityEngine;

public class Player_Hammer : MonoBehaviour
{
    public Animator anim;
    public Transform attackPoint;
    public LayerMask enemyLayer;
    public PlayerMovement playerMovement;

    private float timer;
    private void Update()
    {
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
        playerMovement.isSmashing = true;
        anim.SetBool("isSmashing", true);
        timer = StatsManager.Instance.coolDown;
    }

    public void SmashDamage()
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
