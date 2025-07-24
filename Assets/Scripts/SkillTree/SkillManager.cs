using System;
using TMPro;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public PowerupSpawner powerupSpawner;
    public static event Action updateStats;

    void OnEnable()
    {
        SkillSlot.OnAbilityPointSpent += HandleAbilityPointSpent;
    }

    void OnDisable()
    {
        SkillSlot.OnAbilityPointSpent -= HandleAbilityPointSpent;
    }

    private void HandleAbilityPointSpent(SkillSlot slot)
    {
        string skillName = slot.skillSO.skillName;

        switch (skillName)
        {
            case "MaxHealth":
                StatsManager.Instance.UpdateMaxHealth(1);
                break;
            case "ArcherMode":
                StatsManager.Instance.archer = true;
                break;
            case "HammerMode":
                StatsManager.Instance.hammer = true;
                break;
            case "Agility":
                StatsManager.Instance.swordCoolDown *= 0.9f; // reload speed halved if upgraded 5x
                StatsManager.Instance.hammerCoolDown *= 0.9f;
                StatsManager.Instance.shootCooldown *= 0.9f;
                break;
            case "Speed":
                StatsManager.Instance.speed *= 1.15f; // speed doubles if upgraded 5x
                StatsManager.Instance.speedBoostMultiplier *= 1.15f; 
                break;
            case "Luck":
                powerupSpawner.IncreaseLuck();
                break;
            case "Strength":
                StatsManager.Instance.damage += 1;
                StatsManager.Instance.arrowDamage += 1;
                StatsManager.Instance.knockbackForce *= 1.1f;
                StatsManager.Instance.stunTime *= 1.1f;
                StatsManager.Instance.arrowKnockbackForce *= 1.1f;
                StatsManager.Instance.arrowKnockbackTime *= 1.1f;
                StatsManager.Instance.arrowMaxDistance += 1;
                StatsManager.Instance.arrowSpeed += 0.5f;
                break;
            case "CurrentHealth":
                StatsManager.Instance.currentHealth += 1; // not showing animation
                break;
            case "LongRangeArcher":
                StatsManager.Instance.arrowMaxDistance += 6;
                StatsManager.Instance.arrowSpeed += 4;
                StatsManager.Instance.arrowStunTime += 0.3f;
                StatsManager.Instance.arrowKnockbackForce += 5;
                StatsManager.Instance.arrowDamage += 1;
                break;
            case "HealthRegen":
                StatsManager.Instance.regenHealth = true;
                break;
            case "Shield":
                StatsManager.Instance.shieldAbility = true;
                StatsManager.Instance.shieldActive = true;
                break;
            case "IQ":
                StatsManager.Instance.arrowMaxDistance += 1.5f;
                StatsManager.Instance.swordRange += 0.15f;
                StatsManager.Instance.hammerRange += 0.25f;
                StatsManager.Instance.stunTime += 0.05f;
                StatsManager.Instance.arrowStunTime += 0.07f;
                break;
            case "Stealth":
                StatsManager.Instance.stealth = true;
                StatsManager.Instance.stealthUsed = false;
                StatsManager.Instance.stealthLevel += 1;
                break;
            default:
                Debug.LogWarning("Unknown skill: " + skillName);
                break;
        }

        updateStats();
    }
}
