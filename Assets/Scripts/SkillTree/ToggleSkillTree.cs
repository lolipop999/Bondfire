using System;
using System.Collections;
using UnityEngine;

public class ToggleSkillTree : MonoBehaviour
{
    public CanvasGroup skillsCanvas;
    public CanvasGroup statsCanvas;
    public static event Action statsOn;
    private PlayerMovement playerMovement;
    private bool statsOpen = false;

    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerMovement.isActive) return;
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
        StartCoroutine(UIFader.Instance.FadeCanvas(statsCanvas, 1, 0, 0.05f));
        yield return StartCoroutine(UIFader.Instance.FadeCanvas(skillsCanvas, 1, 0, 0.05f));
        skillsCanvas.blocksRaycasts = false;
        skillsCanvas.interactable = false;
        statsOpen = false;
    }

    IEnumerator OpenStats()
    {
        StartCoroutine(UIFader.Instance.FadeCanvas(skillsCanvas, 0, 1, 0.1f));
        StartCoroutine(UIFader.Instance.FadeCanvas(statsCanvas, 0, 1, 0.1f));
        FXManager.Instance.PlaySound(FXManager.Instance.openStats, 0.2f);
        skillsCanvas.blocksRaycasts = true;
        skillsCanvas.interactable = true;
        statsOn(); 
        statsOpen = true;

        yield return new WaitForSeconds(0.5f);

        Time.timeScale = 0;
    }
}
