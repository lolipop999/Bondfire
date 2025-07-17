using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFader : MonoBehaviour
{
    public static UIFader Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public IEnumerator FadeCanvas(CanvasGroup canvas, float from, float to, float duration)
    {
        float timer = 0f;
        canvas.alpha = from;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            canvas.alpha = Mathf.Lerp(from, to, timer / duration);
            yield return null;
        }

        canvas.alpha = to;
    }

    public IEnumerator FadeSpriteTo(SpriteRenderer sr, float targetAlpha, float duration)
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
}
