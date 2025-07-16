using System;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    public EnemyData data;
    public delegate void MonsterDefeated(int exp);
    public static event MonsterDefeated OnMonsterDefeated;
    public static event Action<GameObject> OnEnemyDeath;
    private int currentHealth;

    private void Start()
    {
        currentHealth = data.maxHealth;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > data.maxHealth)
        {
            currentHealth = data.maxHealth;
        }
        if (currentHealth <= 0)
        {
            EnemyDead();
        }
    }

    private void EnemyDead()
    {
        OnMonsterDefeated(data.expReward);
        OnEnemyDeath?.Invoke(gameObject);
        Destroy(gameObject);
    }
}
