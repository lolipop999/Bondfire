using UnityEngine;

public class Health_Powerup : MonoBehaviour
{

    public Animator textUpdate;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (StatsManager.Instance.currentHealth < StatsManager.Instance.maxHealth)
            {
                Debug.Log("healing player");
                textUpdate.SetTrigger("UpdateText");
                StatsManager.Instance.currentHealth++;
                StatsManager.Instance.UpdateMaxHealth(0);
            }
            else
            {
                Debug.Log("full health");
            }
            Destroy(gameObject);
        }
    }
}
