using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class ExpManager : MonoBehaviour
{
    public int level;
    public int currentExp;
    public int expToLevel = 10;
    public float expGrowthMultiplier = 1.2f;
    public Slider expSlider;
    public TMP_Text currentLevelText;
    public CanvasGroup powerupCanvas;
    private int expMultiplier = 1;
    private float fadeDuration = 0.5f;

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
        // Fade IN
        yield return StartCoroutine(FadeCanvas(powerupCanvas, 0f, 1f, fadeDuration));

        // Keep the canvas visible for the power‑up’s active time
        yield return new WaitForSeconds(duration);

        // Fade OUT
        yield return StartCoroutine(FadeCanvas(powerupCanvas, 1f, 0f, fadeDuration));

        expMultiplier = 1;
    }

    private IEnumerator FadeCanvas(CanvasGroup cg, float from, float to, float time)
    {
        cg.alpha = from;
        float t = 0f;

        while (t < time)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, t / time);
            yield return null;
        }

        cg.alpha = to;
    }
}
