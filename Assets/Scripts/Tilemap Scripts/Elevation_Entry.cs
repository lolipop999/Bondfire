using Unity.VisualScripting;
using UnityEngine;

public class Elevation_Entry : MonoBehaviour
{
    public Collider2D[] mountainColliders;
    public Collider2D[] boundaryColliders;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            foreach (Collider2D mountain in mountainColliders)
            {
                Physics2D.IgnoreCollision(collision, mountain, true);
            }
            foreach (Collider2D boundary in boundaryColliders)
            {
                boundary.enabled = true;
                Physics2D.IgnoreCollision(collision, boundary, false);
            }

            collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 15;
        }

    }
}
