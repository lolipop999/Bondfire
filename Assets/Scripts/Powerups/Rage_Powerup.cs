using UnityEngine;

public class Rage_Powerup : MonoBehaviour
{
    public float boostDuration;
    private PowerupUI powerupUI;
    void Start()
    {
        powerupUI = FindFirstObjectByType<PowerupUI>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StatsManager.Instance.StartCoroutine(StatsManager.Instance.RageEffect(boostDuration));
            powerupUI.ShowPowerup(boostDuration, "Rage");
            Destroy(gameObject);
        }
    }
}
