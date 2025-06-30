using UnityEngine;

public class Health_Powerup : MonoBehaviour
{
    private PlayerEffects playerEffects;

    void Start()
    {
        if (playerEffects == null)
        {
            playerEffects = GameObject.FindWithTag("Player").GetComponent<PlayerEffects>();
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (StatsManager.Instance.currentHealth < StatsManager.Instance.maxHealth)
            {
                StatsManager.Instance.currentHealth++;
                playerEffects.EnableHealthBoostParticles();
                StatsManager.Instance.UpdateMaxHealth(0);
            }
            Destroy(gameObject);
        }
    }
}
