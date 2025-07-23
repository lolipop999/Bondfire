using UnityEngine;

public class PowerupInstance : MonoBehaviour
{
    private PowerupEntry powerupEntry;
    private PowerupSpawner spawner;
    private float lifetime = 20f; // Time until despawn

    public void Setup(PowerupEntry entry, PowerupSpawner sourceSpawner)
    {
        powerupEntry = entry;
        spawner = sourceSpawner;
        Invoke(nameof(DestroySelf), lifetime);
    }

    private void OnDestroy()
    {
        if (spawner != null && powerupEntry != null)
        {
            spawner.OnPowerupDestroyed(powerupEntry);
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
