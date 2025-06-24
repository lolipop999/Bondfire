using UnityEngine;

public class Elevation_Exit : MonoBehaviour
{
    public Collider2D[] mountainColliders;
    public Collider2D[] boundaryColliders;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            foreach (Collider2D mountain in mountainColliders)
            {
                Physics2D.IgnoreCollision(collision, mountain, false);
            }
            foreach (Collider2D boundary in boundaryColliders)
            {
                Physics2D.IgnoreCollision(collision, boundary, true);
            }

            collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }

    }
}
