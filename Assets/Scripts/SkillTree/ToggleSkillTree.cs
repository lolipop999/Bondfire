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
                statsCanvas.alpha = 0;
                statsOpen = false;
            }
            else
            {
                Time.timeScale = 0;
                skillsCanvas.alpha = 1;
                skillsCanvas.blocksRaycasts = true;
                statsOn();
                statsCanvas.alpha = 1;
                statsOpen = true;
            }
        }
    }
}
