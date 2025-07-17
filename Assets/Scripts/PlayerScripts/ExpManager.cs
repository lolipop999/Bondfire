using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class ExpManager : MonoBehaviour
{
    public int expToLevel = 10;
    public float expGrowthMultiplier = 1.2f;
    public Slider expSlider;
    public TMP_Text currentLevelText;
    public CanvasGroup powerupCanvas;
    private int level = 0;
    private int currentExp = 0;
    private int expMultiplier = 1;

    public static event Action<int> OnLevelUp;

    private void Start()
    {
        UpdateUI();
    }

    public void GainExperience(int amount)
    {
        currentExp += amount * expMultiplier;
        if (currentExp >= expToLevel)
        {
            LevelUp();
        }
        UpdateUI();
    }

    public int GetLevel()
    {
        return level;   
    }

    private void LevelUp()
    {
        level++;
        FXManager.Instance.PlaySound(FXManager.Instance.levelUp);
        currentExp -= expToLevel;
        expToLevel = Mathf.RoundToInt(expToLevel * expGrowthMultiplier);
        OnLevelUp?.Invoke(1);
    }

    public void UpdateUI()
    {
        expSlider.maxValue = expToLevel;
        expSlider.value = currentExp;
        currentLevelText.text = "Level: " + level;
    }

    private void OnEnable()
    {
        Enemy_Health.OnMonsterDefeated += GainExperience;
    }
    private void OnDisable()
    {
        Enemy_Health.OnMonsterDefeated -= GainExperience;
    }

    public IEnumerator doubleXP(float duration)
    {
        expMultiplier = 2;

        yield return new WaitForSeconds(duration);

        expMultiplier = 1;
    }
}
