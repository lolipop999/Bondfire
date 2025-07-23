using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

public class StatsUI : MonoBehaviour
{
    // TO ADD A NEW STAT:
    // 1. create a new void method to Update stat
    // 2. add that method to UpdateAllStats()
    // 3. add statsSlot to statsCanvas

    public GameObject[] statsSlots;
    public CanvasGroup statsCanvas;
    public List<SkillSlot> skills;

    void Start()
    {
        UpdateAllStats();
    }

    void OnEnable()
    {
        ToggleSkillTree.statsOn += UpdateAllStats;
        SkillManager.updateStats += UpdateAllStats;
    }

    void OnDisable()
    {
        ToggleSkillTree.statsOn -= UpdateAllStats;
        SkillManager.updateStats -= UpdateAllStats;
    }

    public void UpdateHealth()
    {
        statsSlots[0].GetComponentInChildren<TMP_Text>().text = "Max Health: " + StatsManager.Instance.maxHealth;
    }

    public void UpdateCurrHealth()
    {
        StatsManager statsManager = FindFirstObjectByType<StatsManager>();
        statsManager.UpdateMaxHealth(0);
        statsSlots[1].GetComponentInChildren<TMP_Text>().text = "Current Health: " + StatsManager.Instance.currentHealth;
    }
    
    public void UpdateDamage()
    {
        statsSlots[2].GetComponentInChildren<TMP_Text>().text = "Damage: " + StatsManager.Instance.damage;
    }

    public void UpdateIQ()
    {
        int level = skills[0].currentLevel;
        if (skills[1].isUnlocked)
        {
            level += skills[1].currentLevel;
        }
        int iqAmount = 0;
        for (int i = 0; i < level; i++)
        {
            iqAmount += 35;
        }
        if (level == 5)
        {
            iqAmount = 200;
        }
        statsSlots[3].GetComponentInChildren<TMP_Text>().text = "IQ: " + Math.Max(1, iqAmount);
    }

    public void UpdateSpeed()
    {
        int level = skills[2].currentLevel;
        if (skills[3].isUnlocked)
        {
            level += skills[3].currentLevel;
        }
        statsSlots[4].GetComponentInChildren<TMP_Text>().text = "Speed LVL: " + level;
    }

    public void UpdateStrength()
    {
        int level = skills[4].currentLevel;
        if (skills[5].isUnlocked)
        {
            level += skills[5].currentLevel;
        }
        statsSlots[5].GetComponentInChildren<TMP_Text>().text = "Strength LVL: " + level;
    }

    public void UpdateStealth()
    {
        int level = skills[6].currentLevel;
        statsSlots[6].GetComponentInChildren<TMP_Text>().text = "Stealth LVL: " + level;
    }

    public void UpdateAgility()
    {
        int level = skills[7].currentLevel;
        statsSlots[7].GetComponentInChildren<TMP_Text>().text = "Agility LVL: " + level;
    }


    public void UpdateAllStats()
    {
        UpdateDamage();
        UpdateSpeed();
        UpdateHealth();
        UpdateCurrHealth();
        UpdateStealth();
        UpdateIQ();
        UpdateAgility();
        UpdateStrength();
    }
}
