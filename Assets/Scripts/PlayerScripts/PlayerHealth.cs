using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public TMP_Text healthText;
    public Animator healthTextAnim;
    public SpriteRenderer playerShield;

    private void Start()
    {
        healthText.text = "HP: " + StatsManager.Instance.currentHealth + " / " + StatsManager.Instance.maxHealth;
        playerShield.sortingOrder = -5;
    }

    void Update()
    {
        if (StatsManager.Instance.shieldActive)
        {
            playerShield.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
        }
        else
        {
            playerShield.sortingOrder = -5;
        }
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0 && StatsManager.Instance.shieldActive) // enemy attacked 
        {
            StatsManager.Instance.shieldActive = false;
        }
        else
        {
            StatsManager.Instance.currentHealth += amount;
            HealthAnimation();

            healthText.text = "HP: " + StatsManager.Instance.currentHealth + " / " + StatsManager.Instance.maxHealth;

            if (StatsManager.Instance.currentHealth <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
    public void HealthAnimation()
    {
        healthTextAnim.Play("TextUpdate");
    }
}
