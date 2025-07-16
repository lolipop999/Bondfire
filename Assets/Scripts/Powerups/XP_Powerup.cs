using UnityEngine;

public class XP_Powerup : MonoBehaviour
{
    // 
    public float duration;
    private ExpManager expManager;
    void Start()
    {
        expManager = FindFirstObjectByType<ExpManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            expManager.StartCoroutine(expManager.doubleXP(duration));

            Destroy(gameObject);
        }
    }
}
