using System.Collections;
using UnityEngine;

public class Speed_Powerup : MonoBehaviour
{
    public float speedBoost;
    public float boostDuration;
    private PowerupUI powerupUI;
    void Start()
    {
        powerupUI = FindFirstObjectByType<PowerupUI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !powerupUI.isActive)
        {
            StatsManager.Instance.StartCoroutine(StatsManager.Instance.SpeedBoost(speedBoost, boostDuration));
            powerupUI.ShowPowerup(boostDuration, "Speed Boost");
            Destroy(gameObject);
        }
    }
}
