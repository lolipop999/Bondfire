using UnityEngine;
using TMPro;
using System;

public class StatsUI : MonoBehaviour
{
    // TO ADD A NEW STAT:
    // 1. create a new void method to Update stat
    // 2. add that method to UpdateAllStats()
    // 3. add statsSlot to statsCanvas

    public GameObject[] statsSlots;
    public CanvasGroup statsCanvas;

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
    public void UpdateDamage()
    {
        statsSlots[1].GetComponentInChildren<TMP_Text>().text = "Damage: " + StatsManager.Instance.damage;
    }

    public void UpdateSpeed()
    {
        statsSlots[2].GetComponentInChildren<TMP_Text>().text = "Speed: " + Convert.ToInt32(StatsManager.Instance.speed);
    }

    public void UpdateAllStats()
    {
        UpdateDamage();
        UpdateSpeed();
        UpdateHealth();
    }
}
