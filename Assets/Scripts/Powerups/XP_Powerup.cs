using UnityEngine;

public class XP_Powerup : MonoBehaviour
{
    public float duration;
    public ExpManager expManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            expManager.StartCoroutine(expManager.doubleXP(duration));

            Destroy(gameObject);
        }
    }
}
