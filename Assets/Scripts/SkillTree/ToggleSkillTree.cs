using System;
using UnityEngine;

public class ToggleSkillTree : MonoBehaviour
{
    public CanvasGroup skillsCanvas;
    private bool statsOpen = false;
    public CanvasGroup statsCanvas;
    public static event Action statsOn;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("ToggleStats"))
        {
            if (statsOpen)
            {
                Time.timeScale = 1;
                StartCoroutine(UIFader.Instance.FadeCanvas(skillsCanvas, 1, 0, 0.5f));
                skillsCanvas.blocksRaycasts = false;
                skillsCanvas.interactable = false;
                StartCoroutine(UIFader.Instance.FadeCanvas(statsCanvas, 1, 0, 0.5f));
                statsOpen = false;
            }
            else
            {
                Time.timeScale = 0;
                FXManager.Instance.PlaySound(FXManager.Instance.openStats, 0.3f);
                StartCoroutine(UIFader.Instance.FadeCanvas(skillsCanvas, 0, 1, 0.5f));
                skillsCanvas.blocksRaycasts = true;
                skillsCanvas.interactable = true;
                statsOn();
                StartCoroutine(UIFader.Instance.FadeCanvas(statsCanvas, 0, 1, 0.5f));
                statsOpen = true;
            }
        }
    }
}
