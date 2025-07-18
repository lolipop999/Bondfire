using System;
using System.Collections;
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
                StartCoroutine(CloseStats());
            }
            else
            {
                StartCoroutine(OpenStats());
            }
        }
    }

    IEnumerator CloseStats()
    {
        StartCoroutine(UIFader.Instance.FadeCanvas(skillsCanvas, 1, 0, 0.5f));
        yield return StartCoroutine(UIFader.Instance.FadeCanvas(statsCanvas, 1, 0, 0.5f));
        skillsCanvas.blocksRaycasts = false;
        skillsCanvas.interactable = false;
        statsOpen = false;
    }

    IEnumerator OpenStats()
    {
        StartCoroutine(UIFader.Instance.FadeCanvas(skillsCanvas, 0, 1, 0.5f));
        StartCoroutine(UIFader.Instance.FadeCanvas(statsCanvas, 0, 1, 0.5f));
        FXManager.Instance.PlaySound(FXManager.Instance.openStats, 0.3f);
        skillsCanvas.blocksRaycasts = true;
        skillsCanvas.interactable = true;
        statsOn();
        statsOpen = true;

        yield return new WaitForSeconds(0.5f);

        Time.timeScale = 0;
    }
}
