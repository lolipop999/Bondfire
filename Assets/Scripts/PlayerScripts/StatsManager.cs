using UnityEngine;
using TMPro;
using System.Collections;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;
    public TMP_Text healthText;

    [Header("Combat Stats")]
    public int damage;
    public float weaponRange;
    public float knockbackForce;
    public float knockbackTime;
    public float stunTime;
    public float coolDown;

    [Header("Movement Stats")]
    public float speed;

    [Header("Health Stats")]
    public int maxHealth;
    public int currentHealth;

    [Header("Combat Abilities")]
    public bool archer = false;
    public bool hammer = false;

    private PlayerHealth playerHealth;
    private PlayerEffects playerEffects;

    void Start()
    {
        if (playerHealth == null)
        {
            playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        }
        if (playerEffects == null)
        {
            playerEffects = GameObject.FindWithTag("Player").GetComponent<PlayerEffects>();
        }   
    }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateMaxHealth(int amount)
    {
        maxHealth += amount;
        healthText.text = "HP: " + currentHealth + " / " + maxHealth;
        playerHealth.HealthAnimation();
    }

    public IEnumerator SpeedBoost(float boostAmount, float duration)
    {
        playerEffects.EnableSpeedBoostTrail(true);
        float originalSpeed = speed;
        speed = originalSpeed + boostAmount;

        yield return new WaitForSeconds(duration);

        speed = originalSpeed;
        playerEffects.EnableSpeedBoostTrail(false);
    }
}
