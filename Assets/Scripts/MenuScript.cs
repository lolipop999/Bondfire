using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public CanvasGroup infoCanvas;
    public CanvasGroup mainMenu;
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ShowInfo()
    {
        infoCanvas.alpha = 1;
        infoCanvas.interactable = true;
        infoCanvas.blocksRaycasts = true;

        mainMenu.alpha = 0;
        mainMenu.interactable = false;
        mainMenu.blocksRaycasts = false;
    }

    public void Back()
    {
        infoCanvas.alpha = 0;
        infoCanvas.interactable = false;
        infoCanvas.blocksRaycasts = false;

        mainMenu.alpha = 1;
        mainMenu.interactable = true;
        mainMenu.blocksRaycasts = true;
    }
}
