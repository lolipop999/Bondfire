using UnityEngine;
using UnityEngine.Playables;

public class CutSceneManager : MonoBehaviour
{
    public PlayableDirector director;
    public SpriteRenderer player;
    public GameObject enemySpawner;
    public GameObject UI;
    public GameObject startPlayer;
    public PlayerMovement playerMovement;
    public CanvasGroup playerUI;
    void Start()
    {
        enemySpawner.SetActive(false);
        player.sortingOrder = -5;
        UI.SetActive(false);
        playerMovement.EnableKnockback();
        playerUI.alpha = 0;
        director.stopped += OnTimelineFinished;
        director.Play();
    }

    void OnTimelineFinished(PlayableDirector obj)
    {
        enemySpawner.SetActive(true);
        UI.SetActive(true);
        startPlayer.SetActive(false);
        playerMovement.DisableKnockback();
        playerUI.alpha = 1;
        player.sortingOrder = 5;
    }
}
