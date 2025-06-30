using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemies/New Enemy Data")]
public class EnemyData : ScriptableObject
{
    public float speed = 3;
    public int maxHealth = 3;
    public int damage = 1;
    public float attackRange = 2;
    public float attackCoolDown = 1;
    public int expReward = 3;
    public float knockbackForce;
    public float stunTime;
    public float playerDetectRange = 5;
    public float idleTime = 5f; // upper time of how long enemy idles
    public float forcedChaseDuration = 5f; // how long enemy blindly chases after idle
}

