using UnityEngine;
using TMPro;

public class SkillTreeManager : MonoBehaviour
{
    public SkillSlot[] skillSlots;
    public TMP_Text pointsText;

    private void Start()
    {
        foreach (SkillSlot slot in skillSlots)
        {
            slot.skillButton.onClick.AddListener(() => CheckAvailablePoints(slot));
        }
        UpdateAbilityPoints(0);
    }

    private void CheckAvailablePoints(SkillSlot slot)
    {
        if (StatsManager.Instance.skillPoints >= slot.skillSO.cost)
        {
            slot.TryUpgradeSkill();
        }
    }

    public void UpdateAbilityPoints(int amount)
    {
        StatsManager.Instance.skillPoints += amount;
        pointsText.text = "Points: " + StatsManager.Instance.skillPoints;
    }

    public void OnEnable()
    {
        SkillSlot.OnAbilityPointSpent += HandleAbilityPointsSpent;
        SkillSlot.OnSkillMaxed += HandleSkillMaxed;
        ExpManager.OnLevelUp += UpdateAbilityPoints;
    }
    public void OnDisable()
    {
        SkillSlot.OnAbilityPointSpent -= HandleAbilityPointsSpent;
        SkillSlot.OnSkillMaxed -= HandleSkillMaxed;
        ExpManager.OnLevelUp -= UpdateAbilityPoints;
    }

    private void HandleAbilityPointsSpent(SkillSlot skillSlot)
    {
        if (StatsManager.Instance.skillPoints > 0)
        {
            UpdateAbilityPoints(-skillSlot.skillSO.cost);
        }
    }
    
    private void HandleSkillMaxed(SkillSlot skillSlot)
    { 
        foreach (SkillSlot slot in skillSlots)
        {
            if (!slot.isUnlocked && slot.CanUnlockSkill())
            {
                slot.Unlock();
            }
        }
    }
}
