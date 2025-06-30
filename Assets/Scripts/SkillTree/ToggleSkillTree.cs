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
                skillsCanvas.alpha = 0;
                skillsCanvas.blocksRaycasts = false;
                skillsCanvas.interactable = false;
                statsCanvas.alpha = 0;
                statsOpen = false;
            }
            else
            {
                Time.timeScale = 0;
                FXManager.Instance.PlaySound(FXManager.Instance.openStats, 0.3f);
                skillsCanvas.alpha = 1;
                skillsCanvas.blocksRaycasts = true;
                skillsCanvas.interactable = true;
                statsOn();
                statsCanvas.alpha = 1;
                statsOpen = true;
            }
        }
    }
}
