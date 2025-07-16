using UnityEngine;
using System.Collections;

using TMPro;
using UnityEngine.Rendering;
using System.Collections.Generic;
using Unity.VisualScripting; // Add this at the top

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public int numEnemies = 5;
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
    public CutSceneManager cutSceneManager;

    public TMP_Text waveAnnouncementText;
    public CanvasGroup waveCanvas;
    private float fadeDuration = 0.5f;
    private int currentWaveIndex = 0;
    private int enemyCount = 0;
    private bool isPaused = false;
    private List<Enemy_Movement> activeEnemies = new List<Enemy_Movement>();
    public int numberToSpawn = 15;
    public float delayToSpawn = 2;
    public int increaseSpawn = 2;
    public float decreaseDelayBy = 0.3f;
    public float increaseTimeBetweenWaves;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            while (isPaused) yield return null;

            yield return StartCoroutine(ShowWaveAnnouncement(currentWaveIndex + 1));
            ResetAbilities();
            
            Wave wave = waves[currentWaveIndex];
            for (int i = 0; i < wave.numEnemies; i++)
            {
                SpawnEnemy();
                
                yield return new WaitForSeconds(wave.spawnInterval);
            }

            currentWaveIndex++;
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }
    private void ResetAbilities()
    {
        // resets abilities every wave
        if (StatsManager.Instance.regenHealth)
        {
            if (StatsManager.Instance.currentHealth < StatsManager.Instance.maxHealth)
            {
                StatsManager.Instance.currentHealth += 1;
            }
        }
        if (StatsManager.Instance.shieldAbility)
        {
            StatsManager.Instance.shieldActive = true;
        }
        if (StatsManager.Instance.stealth)
        {
            StatsManager.Instance.stealthUsed = false;
        }
    }

    public IEnumerator SpawnInfiniteWaves()
    {
        while (true)
        {
            currentWaveIndex++;

            yield return StartCoroutine(ShowWaveAnnouncement(currentWaveIndex));
            ResetAbilities();

            for (int i = 0; i < numberToSpawn; i++)
            {
                SpawnEnemy();

                yield return new WaitForSeconds(delayToSpawn);
            }

            numberToSpawn += increaseSpawn;
            delayToSpawn -= decreaseDelayBy;
            timeBetweenWaves += increaseTimeBetweenWaves;
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }


    public int GetCurrentWave()
    {
        return currentWaveIndex;
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
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemyObj = Instantiate(GetRandomEnemy(), spawnPoint.position, Quaternion.identity);
        enemyCount++;

        Enemy_Movement movement = enemyObj.GetComponent<Enemy_Movement>();
        activeEnemies.Add(movement);

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

    private void HandleEnemyDeath(GameObject enemyObj)
    {
        enemyCount--;

        Enemy_Movement movement = enemyObj.GetComponent<Enemy_Movement>();
        if (movement != null)
        {
            activeEnemies.Remove(movement);
        }

        if (!isPaused && currentWaveIndex == waves.Length && enemyCount <= 0)
        {

            cutSceneManager.TriggerEndCutscene(true);
        }
    }

    public void ResumeSpawner()
    {
        isPaused = false;
    }
    public void PauseSpawner()
    {
        isPaused = true;
        foreach (var enemy in activeEnemies)
        {
            enemy.stopMoving = true; // or call enemy.DisableMovement() if you have a method
        }
    }


}
