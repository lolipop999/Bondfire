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
    public float swordCoolDown = 1.2f;
    public float hammerCoolDown = 2f;
    public float shootCooldown = 0.5f;
    [Header("Archer Stats")]
    public float arrowSpeed = 7;
    public int arrowDamage = 1;
    public float arrowKnockbackForce = 5f;
    public float arrowKnockbackTime = 0.2f;
    public float arrowStunTime = 0.3f;
    public float arrowMaxDistance = 4;

    [Header("Movement Stats")]
    public float speed;
    public float speedBoostMultiplier = 1;

    [Header("Health Stats")]
    public int maxHealth;
    public int currentHealth;
    public bool regenHealth = false;
    public bool shieldAbility = false;
    public bool shieldActive = false;

    [Header("Combat Abilities")]
    public bool archer = false;
    public bool hammer = false;
    public bool stealth = false;
    public bool stealthUsed = true;
    public int stealthLevel = 0;

    [Header("Combat Abilities")]
    public int skillPoints = 10;

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
        speed = originalSpeed + (boostAmount * speedBoostMultiplier);

        yield return new WaitForSeconds(duration);

        speed = originalSpeed;
        playerEffects.EnableSpeedBoostTrail(false);
    }
}
