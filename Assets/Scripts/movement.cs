using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour
{
    public float speed = 5;
    public Rigidbody2D rb;

    private int facingDirection = 1;
    private float originalSpeed;

    void Start() {
        originalSpeed = speed;
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal > 0 && facingDirection == -1 || horizontal < 0 && facingDirection == 1) {
            Flip();
        }

        rb.linearVelocity = new Vector2(horizontal, vertical) * speed;
    }

    void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        facingDirection *= -1;
    }

    public IEnumerator TemporarySpeedBoost(float boostedSpeed, float duration)
    {
        speed = boostedSpeed;                 // Change speed
        yield return new WaitForSeconds(duration);  // Wait X seconds
        speed = originalSpeed;               // Reset speed after wait
    }
}
