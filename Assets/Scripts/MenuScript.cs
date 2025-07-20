using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public CanvasGroup infoCanvas;
    public CanvasGroup mainMenu;
    public CanvasGroup creditsCanvas;
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
    }

    public void ShowCredits()
    {
        StartCoroutine(UIFader.Instance.FadeCanvas(creditsCanvas, 0, 1, 0.5f));
        creditsCanvas.interactable = true;
        creditsCanvas.blocksRaycasts = true;

        StartCoroutine(UIFader.Instance.FadeCanvas(mainMenu, 1, 0, 0.5f));
        mainMenu.interactable = false;
        mainMenu.blocksRaycasts = false;
    }

    public void Back()
    {
        StartCoroutine(UIFader.Instance.FadeCanvas(infoCanvas, 1, 0, 0.5f));
        infoCanvas.interactable = false;
        infoCanvas.blocksRaycasts = false;

        StartCoroutine(UIFader.Instance.FadeCanvas(creditsCanvas, 1, 0, 0.5f));
        creditsCanvas.interactable = false;
        creditsCanvas.blocksRaycasts = false;

        mainMenu.alpha = 1;
        mainMenu.interactable = true;
        mainMenu.blocksRaycasts = true;
    }
}
