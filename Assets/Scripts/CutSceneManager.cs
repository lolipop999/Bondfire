using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;


public class CutSceneManager : MonoBehaviour
{
    public CanvasGroup playerXP;
    public GameObject startPlayer;
    public PlayerMovement playerMovement;
    public CanvasGroup playerUI;
    public SpriteRenderer deadPlayer;
    public CanvasGroup endInfo;
    public SpriteRenderer playerSprite;
    public TextMeshProUGUI endText;
    public TextMeshProUGUI continueText;
    public TextMeshProUGUI wavesText;
    public ExpManager expManager;
    public EnemySpawner enemyScript;
    public SpriteRenderer loadScreen;
    public PlayableDirector director;
    public GameObject enemySpawner;
    public CanvasGroup pauseCanvas;
    public CanvasGroup enemiesLeftCanvas;
    private bool died;
    private bool wonGame = false;
    private bool inPause = false;
    private bool inEnd = false;

    void Start()
    {
        StartCoroutine(StartGame());
        playerXP.alpha = 0;
        playerMovement.isActive = false;
        playerUI.alpha = 0;
        director.stopped += OnTimelineFinished;
        director.Play();
    }

    private IEnumerator StartGame()
    {
        // player must be alpha 0
        // load screen must be alpha 1
        yield return StartCoroutine(UIFader.Instance.FadeSpriteTo(loadScreen, 0, 3f));
    }

    void OnTimelineFinished(PlayableDirector obj)
    {
        StartCoroutine(UIFader.Instance.FadeCanvas(playerXP, 0, 1, 0.5f));
        StartCoroutine(UIFader.Instance.FadeCanvas(playerUI, 0, 1, 0.5f));
        StartCoroutine(UIFader.Instance.FadeCanvas(enemiesLeftCanvas, 0, 1, 0.5f));
        StartCoroutine(UIFader.Instance.FadeSpriteTo(playerSprite, 1, 0.2f));
        startPlayer.SetActive(false);
        playerMovement.isActive = true;
        enemySpawner.SetActive(true);
    }
    void Update()
    {
        if (Input.GetButtonDown("Pause") && !inPause && !inEnd)
        {
            inPause = true;
            PauseGame();
        }
    }

    public void TriggerEndCutscene(bool win)
    {
        inEnd = true;
        StartCoroutine(UIFader.Instance.FadeCanvas(playerUI, 1, 0, 0.6f));
        StartCoroutine(UIFader.Instance.FadeCanvas(playerXP, 1, 0, 0.6f));
        StartCoroutine(UIFader.Instance.FadeCanvas(enemiesLeftCanvas, 1, 0, 0.6f));
        died = !win;

        if (!died)
        {
            StartCoroutine(WinCutscene());
        }
        else
        {
            StartCoroutine(LoseCutscene());
        }

    }

    private IEnumerator WinCutscene()
    {
        // celebration noise


        yield return new WaitForSeconds(1);
        wonGame = true;
        // end game info
        endText.text = "You Win!";
        wavesText.text = "Waves Survived: " + enemyScript.GetCurrentWave();
        continueText.text = "Continue";

        yield return StartCoroutine(UIFader.Instance.FadeCanvas(endInfo, 0, 1, 1));
        endInfo.interactable = true;
        endInfo.blocksRaycasts = true;
    }

    private IEnumerator LoseCutscene()
    {
        // lose noise

        playerMovement.isActive = false; // prevent player from moving
        // i can still move and shoot after dying

        // stop enemies and powerups from spawning
        enemyScript.PauseSpawner();

        // player disappear
        yield return StartCoroutine(UIFader.Instance.FadeSpriteTo(playerSprite, 0, 1.5f));

        // skull appear
        deadPlayer.GetComponent<Transform>().position = playerSprite.GetComponent<Transform>().position;
        yield return StartCoroutine(UIFader.Instance.FadeSpriteTo(deadPlayer, 1, 1.5f));
        yield return new WaitForSeconds(1f);

        // end game info
        // if player has already won change end text to you died
        if (wonGame)
        {
            endText.text = "You Died";
            wavesText.text = "Waves Survived: " + (enemyScript.GetCurrentWave() - 1);
            continueText.text = "Play Again";
        }
        else
        {
            endText.text = "You Lost";
            wavesText.text = "Waves Survived: " + (enemyScript.GetCurrentWave() - 1) + "/" + enemyScript.GetAllWaves();
            continueText.text = "Retry";
        }

        yield return StartCoroutine(UIFader.Instance.FadeCanvas(endInfo, 0, 1, 1f));
        endInfo.interactable = true;
        endInfo.blocksRaycasts = true;
    }

    public void ResetGame()
    {
        if (wonGame && !died)
        {
            StartCoroutine(ContinuePlaying());
        }
        else // lost game so restart
        {
            StartCoroutine(RestartGame());
        }
    }

    private IEnumerator RestartGame()
    {
        yield return StartCoroutine(FadeOut());

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private IEnumerator ContinuePlaying()
    {
        yield return StartCoroutine(UIFader.Instance.FadeCanvas(endInfo, 1, 0, 1.5f));
        endInfo.interactable = false;
        endInfo.blocksRaycasts = false;

        inEnd = false;

        StartCoroutine(UIFader.Instance.FadeCanvas(playerUI, 0, 1, 0.6f));
        StartCoroutine(UIFader.Instance.FadeCanvas(playerXP, 0, 1, 0.6f));

        yield return new WaitForSeconds(2f);

        StartCoroutine(UIFader.Instance.FadeCanvas(enemiesLeftCanvas, 0, 1, 0.6f));
        StartCoroutine(enemyScript.SpawnInfiniteWaves());
    }

    public void QuitGame()
    {
        StartCoroutine(Quit());
    }

    public void QuitFromPause()
    {
        Time.timeScale = 1;
        StartCoroutine(PauseToMain());
    }

    private IEnumerator PauseToMain()
    {
        yield return StartCoroutine(UIFader.Instance.FadeCanvas(pauseCanvas, 1, 0, 0.5f));
        yield return StartCoroutine(UIFader.Instance.FadeSpriteTo(loadScreen, 1, 3f));
        SceneManager.LoadScene("Menu");
    }

    private IEnumerator Quit()
    {
        yield return StartCoroutine(FadeOut());
        SceneManager.LoadScene("Menu");
    }

    private IEnumerator FadeOut()
    {
        StartCoroutine(UIFader.Instance.FadeCanvas(endInfo, 1, 0, 3f));

        yield return StartCoroutine(UIFader.Instance.FadeSpriteTo(loadScreen, 1, 3f));
    }

    public void ReturnToGame()
    {
        Time.timeScale = 1;
        pauseCanvas.alpha = 0;
        pauseCanvas.interactable = false;
        pauseCanvas.blocksRaycasts = false;
        inPause = false;
    }

    private void PauseGame()
    {
        pauseCanvas.alpha = 1;
        pauseCanvas.interactable = true;
        pauseCanvas.blocksRaycasts = true;
        Time.timeScale = 0;
    }
}
