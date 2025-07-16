using System.Collections;
using TMPro;
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
    private bool wonGame;

    /*     void Start()
            {
                enemySpawner.SetActive(false);
                player.alpha = 0;
                playerXP.alpha = 0;
                playerMovement.EnableKnockback();
                playerUI.alpha = 0;
                director.stopped += OnTimelineFinished;
                director.Play();
            }

            void OnTimelineFinished(PlayableDirector obj)
            {
                enemySpawner.SetActive(true);
                StartCoroutine(FadeCanvas(playerXP, 0, 1, 0.5f));
                StartCoroutine(FadeCanvas(playerUI, 0, 1, 0.5f));
                StartCoroutine(FadeCanvas(player, 0, 1, 0.5f));
                startPlayer.SetActive(false);
                playerMovement.DisableKnockback();
            } */

    public void TriggerEndCutscene(bool win)
    {
        StartCoroutine(FadeCanvas(playerUI, 1, 0, 0.6f));
        StartCoroutine(FadeCanvas(playerXP, 1, 0, 0.6f));
        wonGame = win;
        if (wonGame)
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


        yield return new WaitForSeconds(2f);

        // end game info
        endText.text = "You Win!";
        wavesText.text = "Waves Survived: " + enemyScript.GetCurrentWave();
        continueText.text = "Continue";

        yield return StartCoroutine(FadeCanvas(endInfo, 0, 1, 2f));
        endInfo.interactable = true;
        endInfo.blocksRaycasts = true;
    }

    private IEnumerator LoseCutscene()
    {
        // lose noise

        playerMovement.EnableKnockback(); // prevent player from moving
        // i can still move and shoot after dying

        // stop enemies and powerups from spawning
        enemyScript.PauseSpawner();

        // player disappear
        yield return StartCoroutine(FadeSpriteTo(playerSprite, 0, 1.5f));

        // skull appear
        deadPlayer.GetComponent<Transform>().position = playerSprite.GetComponent<Transform>().position;
        yield return StartCoroutine(FadeSpriteTo(deadPlayer, 1, 1.5f));
        yield return new WaitForSeconds(1f);

        // end game info
        // if player has already won change end text to you died
        endText.text = "You Lose";
        wavesText.text = "Waves Survived: " + (enemyScript.GetCurrentWave() - 1);
        continueText.text = "Retry";
        yield return StartCoroutine(FadeCanvas(endInfo, 0, 1, 2f));
        endInfo.interactable = true;
        endInfo.blocksRaycasts = true;
    }

    private IEnumerator FadeCanvas(CanvasGroup cg, float from, float to, float time)
    {
        float t = 0f;
        cg.alpha = from;
        while (t < time)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, t / time);
            yield return null;
        }
        cg.alpha = to;
    }

    private IEnumerator FadeSpriteTo(SpriteRenderer sr, float targetAlpha, float duration)
    {
        Color color = sr.color;
        float startAlpha = color.a;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, t / duration);
            sr.color = new Color(color.r, color.g, color.b, newAlpha);
            yield return null;
        }

        sr.color = new Color(color.r, color.g, color.b, targetAlpha);
    }
    
    public void ResetGame()
    {
        if (wonGame)
        {
            StartCoroutine(ContinuePlaying());
        }
        else // lost game so restart
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    private IEnumerator ContinuePlaying()
    {
        yield return StartCoroutine(FadeCanvas(endInfo, 1, 0, 3f));
        endInfo.interactable = false;
        endInfo.blocksRaycasts = false;

        StartCoroutine(FadeCanvas(playerUI, 0, 1, 0.6f));
        StartCoroutine(FadeCanvas(playerXP, 0, 1, 0.6f));

        yield return new WaitForSeconds(2f);

        StartCoroutine(enemyScript.SpawnInfiniteWaves());
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Menu");
    }
}
