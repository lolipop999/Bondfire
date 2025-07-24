using System;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class PowerupUI : MonoBehaviour
{
    public TextMeshProUGUI powerupText;
    public CanvasGroup powerupCanvas;
    public Image fillBar;
    private float timeRemaining;
    private float duration;
    public bool isActive = false;

    public void ShowPowerup(float duration, string powerupName)
    {
        this.duration = duration;
        timeRemaining = duration;
        Debug.Log(duration);
        fillBar.fillAmount = 1f;
        powerupText.text = powerupName;

        isActive = true;
        StartCoroutine(UIFader.Instance.FadeCanvas(powerupCanvas, 0, 1, 0.3f));
    }

    void Update()
    {
        if (!isActive) return;
        
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        
        fillBar.fillAmount = timeRemaining / duration;

        if (timeRemaining <= 0f)
        {
            Debug.Log("hide");
            fillBar.fillAmount = 0f;
            isActive = false;
            StartCoroutine(UIFader.Instance.FadeCanvas(powerupCanvas, 1, 0, 0.3f));
        }
    }
}