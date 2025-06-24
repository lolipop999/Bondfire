using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;       // Drag your enemy prefab here in the Inspector
    public Transform[] spawnPoints;      // Set spawn locations in the Inspector
    public float spawnInterval = 5f;     // Time in seconds between spawns
    public int maxEnemies = 10;          // Max enemies allowed in the scene

    public int enemyCount = 0;

    void Start()
    {
        InvokeRepeating("SpawnEnemy", spawnInterval, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (enemyCount >= maxEnemies) return;

        // Pick a random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Instantiate the enemy
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        enemyCount++;
    }

    // reduce enemy count when enemy is destroyed
    private void OnEnable()
    {
        Enemy_Health.OnEnemyDeath += HandleEnemyDeath;
    }
    private void OnDisable()
    {
        Enemy_Health.OnEnemyDeath -= HandleEnemyDeath;
    }

    private void HandleEnemyDeath()
    {
        enemyCount--;
    }
}
