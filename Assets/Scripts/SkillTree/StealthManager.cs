using System.Collections;
using UnityEngine;

public class StealthManager : MonoBehaviour
{
    public static StealthManager Instance;
    public static bool IsStealthActive { get; private set; }

    public delegate void StealthEvent(bool isStealthActive);
    public static event StealthEvent OnStealthChanged;
    private SpriteRenderer playerSprite;

    private void Awake()
    {
        Instance = this;
        playerSprite = GetComponent<SpriteRenderer>();
    }

    public void ActivateStealth(float duration)
    {
        StartCoroutine(StealthRoutine(duration));
    }

    private IEnumerator StealthRoutine(float duration)
    {
        Color origColor = playerSprite.color; // makes player slightly transparent
        Color newColor = origColor;
        newColor.a = 0.6f;
        playerSprite.color = newColor;

        IsStealthActive = true;
        OnStealthChanged?.Invoke(true);

        yield return new WaitForSeconds(duration);

        OnStealthChanged?.Invoke(false);
        IsStealthActive = true;

        playerSprite.color = origColor;
    }
}
