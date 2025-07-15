using UnityEngine;

public class Rage_Powerup : MonoBehaviour
{
    public float boostDuration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (StatsManager.Instance != null)
            {
                StatsManager.Instance.StartCoroutine(StatsManager.Instance.RageEffect(boostDuration));
            }
            else
            {
                Debug.Log("stats manager is null");
            }

            Destroy(gameObject);
        }
    }
}
