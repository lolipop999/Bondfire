using UnityEngine;


public class mushroom : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            movement player = collision.GetComponent<movement>();
            if (player != null)
            {
                player.StartCoroutine(player.TemporarySpeedBoost(10f, 2f));
            }

            // Disable this object (you can use Destroy(gameObject) if you want it gone forever)
            gameObject.SetActive(false);
        }
    }
}
