using System.Collections;
using UnityEngine;

public class Speed_Powerup : MonoBehaviour
{
    public float speedBoost;
    public float boostDuration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (StatsManager.Instance != null)
            {
                StatsManager.Instance.StartCoroutine(StatsManager.Instance.SpeedBoost(speedBoost, boostDuration));
            }
            else
            {
                Debug.Log("stats manager is null");
            }

            Destroy(gameObject);
        }
    }
}
