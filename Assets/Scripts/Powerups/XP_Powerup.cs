using UnityEngine;

public class XP_Powerup : MonoBehaviour
{
    public float duration;
    private ExpManager expManager;
    private PowerupUI powerupUI;
    void Start()
    {
        expManager = FindFirstObjectByType<ExpManager>();
        powerupUI = FindFirstObjectByType<PowerupUI>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            expManager.StartCoroutine(expManager.doubleXP(duration));
            powerupUI.ShowPowerup(duration, "Double XP");

            Destroy(gameObject);
        }
    }
}
