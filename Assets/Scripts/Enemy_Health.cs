using System;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    public int expReward = 3;
    public delegate void MonsterDefeated(int exp);
    public static event MonsterDefeated OnMonsterDefeated;
    public static event Action OnEnemyDeath;
    public int currentHealth;
    public int maxHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (currentHealth <= 0)
        {
            EnemyDead();
            
        }
    }

    private void EnemyDead()
    {
        OnMonsterDefeated(expReward);
        OnEnemyDeath();
        Destroy(gameObject);
    }
}
