using UnityEngine;
using System.Collections;

using TMPro;
using UnityEngine.Rendering;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using Random = UnityEngine.Random; // Add this at the top

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

    [System.Serializable]
    public class EnemyScalingProfile
    {
        public EnemyData enemyData;

        public float speedIncreasePerWave = 0.1f;
        public int healthIncreaseEveryNWaves = 1;
        public int damageIncreaseEveryNWaves = 3;
        public float cooldownReductionPerWave = 0.05f;
        public float idleTimeReduction = 0.2f;
        public float stunTimeIncrease = 0.1f;
        public float knockbackIncrease = 0.3f;
        public float attackRangeIncrease = 0.1f;
        public int xpRewardIncrease = 1;
    }

    public List<EnemyScalingProfile> enemyScalingProfiles;
    public List<EnemyType> enemyTypes;
    public Transform[] spawnPoints;
    public Wave[] waves;
    public float timeBetweenWaves = 3f;
    public CutSceneManager cutSceneManager;

    public TMP_Text waveAnnouncementText;
    public CanvasGroup waveCanvas;
    public int numberToSpawn = 15;
    public float delayToSpawn = 2;
    public int increaseSpawn = 2;
    public float decreaseDelayBy = 0.3f;
    public bool hasWon = false;
    public TMP_Text enemiesRemainingText;
    public Animator enemiesLeftAnim;
    private int currentWaveIndex = 0;
    private int enemyCount = 0;
    private bool isPaused = false;
    private List<Enemy_Movement> activeEnemies = new List<Enemy_Movement>();
    private bool nextWave = false;
    private Dictionary<EnemyData, EnemyData> originalEnemyStats = new Dictionary<EnemyData, EnemyData>();


    void Start()
    {
        SaveOriginalEnemyStats();
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            while (isPaused) yield return null;
            nextWave = false;

            yield return StartCoroutine(ShowWaveAnnouncement(currentWaveIndex + 1));
            ResetAbilities();

            Wave wave = waves[currentWaveIndex];
            for (int i = 0; i < wave.numEnemies; i++)
            {
                SpawnEnemy();

                yield return new WaitForSeconds(wave.spawnInterval);
            }
            currentWaveIndex++;
            while (!nextWave)
            {
                yield return null;
            }
            yield return StartCoroutine(WaveCleared(currentWaveIndex));
            yield return new WaitForSeconds(timeBetweenWaves);
            ScaleEnemyStats(currentWaveIndex);
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
            ScaleEnemyStats(currentWaveIndex);
            nextWave = false;

            yield return StartCoroutine(ShowWaveAnnouncement(currentWaveIndex));
            ResetAbilities();

            for (int i = 0; i < numberToSpawn; i++)
            {
                SpawnEnemy();

                yield return new WaitForSeconds(delayToSpawn);
            }

            numberToSpawn += increaseSpawn;
            if (delayToSpawn > 0.5)
            {
                delayToSpawn -= decreaseDelayBy;
            }
            // wait for all enimies to die to finish before continuing
            while (!nextWave)
            {
                yield return null;
            }

            yield return StartCoroutine(WaveCleared(currentWaveIndex));
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    private void UpdateEnemiesRemainingUI()
    {
        enemiesRemainingText.text = $"Enemies Remaining: {enemyCount}";
        enemiesLeftAnim.Play("TextUpdate");
    }


    private void ScaleEnemyStats(int waveNumber)
    {
        foreach (var profile in enemyScalingProfiles)
        {
            EnemyData data = profile.enemyData;

            data.speed += profile.speedIncreasePerWave;

            if (waveNumber % profile.healthIncreaseEveryNWaves == 0)
            {
                data.maxHealth += 1;
            }
            if (waveNumber % profile.damageIncreaseEveryNWaves == 0)
            {
                data.damage += 1;
            }

            data.idleTime = Math.Max(0, data.idleTime - profile.idleTimeReduction);
            data.stunTime = Math.Min(2f, data.stunTime + profile.stunTimeIncrease);
            data.knockbackForce += profile.knockbackIncrease;
            data.attackRange += profile.attackRangeIncrease;
            data.expReward += profile.xpRewardIncrease;
            data.attackCoolDown = Mathf.Max(0.3f, data.attackCoolDown - profile.cooldownReductionPerWave);
        }
    }


    public int GetCurrentWave()
    {
        return Math.Max(1, currentWaveIndex);
    }

    IEnumerator ShowWaveAnnouncement(int waveNumber)
    {
        waveAnnouncementText.text = $"Wave {waveNumber} Incoming!";
        FadeIn();
        yield return new WaitForSeconds(2f);
        FadeOut();
    }

    IEnumerator WaveCleared(int waveNumber)
    {
        if (hasWon)
        {
            waveAnnouncementText.text = $"Wave {waveNumber} Cleared!";
        }
        else
        {
            waveAnnouncementText.text = $"Wave {waveNumber} / {waves.Length} Cleared!";
        }

        FadeIn();
        yield return new WaitForSeconds(2f);
        FadeOut();
    }

    public void FadeIn()
    {
        StartCoroutine(UIFader.Instance.FadeCanvas(waveCanvas, 0f, 1f, 0.7f));
    }

    public void FadeOut()
    {
        StartCoroutine(UIFader.Instance.FadeCanvas(waveCanvas, 1f, 0f, 0.7f));
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemyObj = Instantiate(GetRandomEnemy(), spawnPoint.position, Quaternion.identity);
        enemyCount++;

        Enemy_Movement movement = enemyObj.GetComponent<Enemy_Movement>();
        activeEnemies.Add(movement);
        UpdateEnemiesRemainingUI();
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
            hasWon = true;
            nextWave = true;
            ResetEnemyStatsToOriginal();
            cutSceneManager.TriggerEndCutscene(true);
        }
        else if (enemyCount <= 0)
        {
            nextWave = true;
        }
        UpdateEnemiesRemainingUI();
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

    public int GetAllWaves()
    {
        return waves.Length;
    }

    private void SaveOriginalEnemyStats()
    {
        foreach (var profile in enemyScalingProfiles)
        {
            var original = ScriptableObject.CreateInstance<EnemyData>();

            original.speed = profile.enemyData.speed;
            original.maxHealth = profile.enemyData.maxHealth;
            original.damage = profile.enemyData.damage;
            original.attackRange = profile.enemyData.attackRange;
            original.attackCoolDown = profile.enemyData.attackCoolDown;
            original.expReward = profile.enemyData.expReward;
            original.knockbackForce = profile.enemyData.knockbackForce;
            original.stunTime = profile.enemyData.stunTime;
            original.playerDetectRange = profile.enemyData.playerDetectRange;
            original.idleTime = profile.enemyData.idleTime;
            original.forcedChaseDuration = profile.enemyData.forcedChaseDuration;

            originalEnemyStats[profile.enemyData] = original;
        }
    }

    public void ResetEnemyStatsToOriginal()
    {
        foreach (var kvp in originalEnemyStats)
        {
            EnemyData data = kvp.Key;
            EnemyData original = kvp.Value;

            data.speed = original.speed;
            data.maxHealth = original.maxHealth;
            data.damage = original.damage;
            data.attackRange = original.attackRange;
            data.attackCoolDown = original.attackCoolDown;
            data.expReward = original.expReward;
            data.knockbackForce = original.knockbackForce;
            data.stunTime = original.stunTime;
            data.playerDetectRange = original.playerDetectRange;
            data.idleTime = original.idleTime;
            data.forcedChaseDuration = original.forcedChaseDuration;
        }
    }

}
