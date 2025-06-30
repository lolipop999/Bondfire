using System;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
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
            default:
                Debug.LogWarning("Unknown skill: " + skillName);
                break;
        }

        updateStats();
    }
}
