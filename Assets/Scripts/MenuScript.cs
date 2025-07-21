using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public CanvasGroup infoCanvas;
    public CanvasGroup mainMenu;
    public CanvasGroup creditsCanvas;
    int ID = 1; // 1 is info open, 2 is credits open
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ShowInfo()
    {
        StartCoroutine(UIFader.Instance.FadeCanvas(mainMenu, 1, 0, 0.5f));
        mainMenu.interactable = false;
        mainMenu.blocksRaycasts = false;

        StartCoroutine(UIFader.Instance.FadeCanvas(infoCanvas, 0, 1, 0.5f));
        infoCanvas.interactable = true;
        infoCanvas.blocksRaycasts = true;
        ID = 1;
    }

    public void ShowCredits()
    {
        StartCoroutine(UIFader.Instance.FadeCanvas(creditsCanvas, 0, 1, 0.5f));
        creditsCanvas.interactable = true;
        creditsCanvas.blocksRaycasts = true;

        StartCoroutine(UIFader.Instance.FadeCanvas(mainMenu, 1, 0, 0.5f));
        mainMenu.interactable = false;
        mainMenu.blocksRaycasts = false;
        ID = 2;
    }

    public void Back()
    {
        if (ID == 1)
        {
            StartCoroutine(UIFader.Instance.FadeCanvas(infoCanvas, 1, 0, 0.5f));
            infoCanvas.interactable = false;
            infoCanvas.blocksRaycasts = false;
        }
        if (ID == 2)
        {
            StartCoroutine(UIFader.Instance.FadeCanvas(creditsCanvas, 1, 0, 0.5f));
            creditsCanvas.interactable = false;
            creditsCanvas.blocksRaycasts = false;
        }

        mainMenu.alpha = 1;
        mainMenu.interactable = true;
        mainMenu.blocksRaycasts = true;
    }
}
