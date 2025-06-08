using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    public Rigidbody2D rb;
    public Animator anim;
    public int facingDirection = 1; // 1 is right, -1 is left

    // Update is called 50x per second
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            flip();
            Debug.Log("Manual Flip Triggered");
        }
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal > 0 && facingDirection == -1 ||
            horizontal < 0 && facingDirection == 1)
        {
            flip();
        }

        anim.SetFloat("horizontal", Math.Abs(horizontal));
        anim.SetFloat("vertical", Math.Abs(vertical));

        rb.linearVelocity = new Vector2(horizontal, vertical) * speed;
    }

    

    void flip()
    {
        facingDirection *= -1;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * facingDirection;
        transform.localScale = scale;
    }
}
