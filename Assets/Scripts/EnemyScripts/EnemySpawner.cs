using UnityEngine;
using System.Collections;

using TMPro;
using UnityEngine.Rendering;
using System.Collections.Generic; // Add this at the top

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public int enemyCount = 5;
        public float spawnInterval = 1f;
    }

    [System.Serializable]
    public class EnemyType
    {
        public GameObject prefab;
        public int weight = 1; // Higher = more likely to spawn
    }

    public List<EnemyType> enemyTypes;
    public Transform[] spawnPoints;
    public Wave[] waves;
    public float timeBetweenWaves = 5f;
    public int maxEnemies = 20;

    public TMP_Text waveAnnouncementText;
    public CanvasGroup waveCanvas;
    private float fadeDuration = 0.5f;
    private int currentWaveIndex = 0;
    private int enemyCount = 0;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            yield return StartCoroutine(ShowWaveAnnouncement(currentWaveIndex + 1));

            Wave wave = waves[currentWaveIndex];
            for (int i = 0; i < wave.enemyCount; i++)
            {
                if (enemyCount < maxEnemies)
                {
                    SpawnEnemy();
                }
                yield return new WaitForSeconds(wave.spawnInterval);
            }

            currentWaveIndex++;
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    IEnumerator ShowWaveAnnouncement(int waveNumber)
    {
        waveAnnouncementText.text = $"Wave {waveNumber} Incoming!";
        FadeIn();
        yield return new WaitForSeconds(2f);
        FadeOut();
    }

    public void FadeIn()
    {
        StartCoroutine(FadeCanvas(0f, 1f));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCanvas(1f, 0f));
    }

    private IEnumerator FadeCanvas(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        waveCanvas.alpha = startAlpha;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            waveCanvas.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            yield return null;
        }
        waveCanvas.alpha = endAlpha;
    }

    void SpawnEnemy()
    {
        if (enemyCount >= maxEnemies) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject selectedPrefab = GetRandomEnemy();

        Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);
        enemyCount++;
    }

    private GameObject GetRandomEnemy()
    {
        int totalWeight = 0;
        foreach (var enemy in enemyTypes)
        {
            totalWeight += enemy.weight;
        }

        int randomValue = Random.Range(0, totalWeight);
        int cumulative = 0;

        foreach (var enemy in enemyTypes)
        {
            cumulative += enemy.weight;
            if (randomValue < cumulative)
            {
                return enemy.prefab;
            }
        }

        return enemyTypes[0].prefab; // fallback (should never hit this)
    }


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
