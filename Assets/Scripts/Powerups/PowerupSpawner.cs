using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerupEntry
{
    public GameObject prefab;
    public int weight = 1; // Higher = more common
    public int maxInstances = 3;
    [HideInInspector] public int currentInstances = 0;
}


public class PowerupSpawner : MonoBehaviour
{
    public List<PowerupEntry> powerups;
    public float minSpawnTime = 3f;
    public float maxSpawnTime = 10f;
    public LayerMask waterLayerMask;

    void Start()
    {
        StartCoroutine(SpawnPowerupRoutine());
    }

    public void IncreaseLuck()
    {
        minSpawnTime *= 0.8f;
        maxSpawnTime *= 0.9f;
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            PowerupEntry chosen = ChooseRandomPowerup();

            if (chosen != null)
            {
                // Get camera bounds
                Camera cam = Camera.main;
                Vector2 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
                Vector2 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

                Vector2 spawnPosition;
                int maxAttempts = 10;
                int attempts = 0;

                do
                {
                    spawnPosition = new Vector2(
                        Random.Range(bottomLeft.x, topRight.x),
                        Random.Range(bottomLeft.y, topRight.y)
                    );
                    attempts++;
                }
                while (Physics2D.OverlapPoint(spawnPosition, waterLayerMask) != null && attempts < maxAttempts);

                if (attempts >= maxAttempts)
                {
                    // Could not find a suitable spawn location, skip this powerup
                    continue;
                }

                GameObject instance = Instantiate(chosen.prefab, spawnPosition, Quaternion.identity);
                chosen.currentInstances++;

                // Attach cleanup script
                PowerupInstance powerup = instance.AddComponent<PowerupInstance>();
                powerup.Setup(chosen, this);
            }
        }
    }

    PowerupEntry ChooseRandomPowerup()
    {
        List<PowerupEntry> weightedPool = new();

        foreach (var entry in powerups)
        {
            if (entry.currentInstances >= entry.maxInstances)
                continue;

            for (int i = 0; i < entry.weight; i++)
                weightedPool.Add(entry);
        }

        if (weightedPool.Count == 0) return null;

        return weightedPool[Random.Range(0, weightedPool.Count)];
    }

    public void OnPowerupDestroyed(PowerupEntry entry)
    {
        entry.currentInstances = Mathf.Max(0, entry.currentInstances - 1);
    }
}
