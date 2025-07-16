using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    public int facingDirection = 1; // 1 is right, -1 is left
    private bool isKnockedBack;
    public bool isShooting;

    void Update() // fastest way to get feedback
    { // handles stealth ability
        if (StatsManager.Instance.stealth && !StatsManager.Instance.stealthUsed)
        {
            if (Input.GetButtonDown("StealthAbility"))
            {
                StealthManager.Instance.ActivateStealth(StatsManager.Instance.stealthLevel * 3);
                StatsManager.Instance.stealthUsed = true;
            }
        }
    }

    // Update is called 50x per second
    void FixedUpdate() 
    { // handles movement
        if (isShooting == true)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else if (isKnockedBack == false)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (horizontal > 0 && facingDirection == -1 ||
                horizontal < 0 && facingDirection == 1)
            {
                flip();
            }

            anim.SetFloat("horizontal", Math.Abs(horizontal));
            anim.SetFloat("vertical", Math.Abs(vertical));

            rb.linearVelocity = new Vector2(horizontal, vertical) * StatsManager.Instance.speed;
        }
    }

    void flip()
    {
        facingDirection *= -1;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * facingDirection;
        transform.localScale = scale;
    }

    public void Knockback(Transform enemy, float force, float stunTime)
    {
        isKnockedBack = true;
        Vector2 direction = (transform.position - enemy.position).normalized;
        rb.linearVelocity = direction * force;
        StartCoroutine(KnockbackCounter(stunTime));
    }

    IEnumerator KnockbackCounter(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }

    public void EnableKnockback()
    {
        isKnockedBack = true;
    }
    public void DisableKnockback()
    {
        isKnockedBack = false;
    }
}
