using System.Collections;
using System.Data.Common;
using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    public EnemyData data;
    public LayerMask playerLayer;
    public Transform attackPoint;
    public GameObject beamPrefab;
    private float beamTime = 0.1f;
    public GameObject impactParticlesPrefab;
    public GameObject rockSmash;

    public void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, data.attackRange, playerLayer);

        if (hits.Length > 0)
        {
            hits[0].GetComponent<PlayerHealth>().ChangeHealth(-data.damage);
            hits[0].GetComponent<PlayerMovement>().Knockback(transform, data.knockbackForce, data.stunTime);

            // wizard
            if (beamPrefab != null)
            {
                WizardAttack(hits[0]);
            }
            // tank
            else if (rockSmash != null)
            {
                TankAttack(hits[0]);
            }
            // minion
            else
            {
                FXManager.Instance.PlaySound(FXManager.Instance.enemyHitPlayer, 0.3f);
            }
        }
    }
    private void OnDrawGizmosSelected() // draws circle of attack point
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, data.attackRange);
    }

    private void TankAttack(Collider2D player)
    {
        // Calculate the midpoint
        Vector3 tankPosition = transform.position;
        Vector3 playerPosition = player.transform.position;
        Vector3 spawnPosition = (tankPosition + playerPosition) / 2f;

        // sound effect
        FXManager.Instance.PlaySound(FXManager.Instance.rockSmash, 0.5f);

        // Instantiate effect
        Instantiate(rockSmash, spawnPosition, Quaternion.identity);
    }

    private void WizardAttack(Collider2D player)
    {
        StartCoroutine(ShootBeam(player.transform));
    }
    
    private IEnumerator ShootBeam(Transform target)
    {
        // sound effect
        FXManager.Instance.PlaySound(FXManager.Instance.wizardZap, 0.5f);
        GameObject beam = Instantiate(beamPrefab);
        LineRenderer lr = beam.GetComponent<LineRenderer>();

        Vector3 start = attackPoint.position;
        Vector3 end = target.position;

        lr.SetPosition(0, start);
        lr.SetPosition(1, start);  // Initially both points at origin

        float growTime = beamTime;  // how long the beam takes to reach full length
        float elapsed = 0f;

        while (elapsed < growTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / growTime); // goes from 0 → 1
            Vector3 current = Vector3.Lerp(start, end, t);
            lr.SetPosition(1, current);
            yield return null;
        }

        Instantiate(impactParticlesPrefab, end, Quaternion.identity);

        // Fade the beam out
        float fadeTime = 0.2f;
        float fadeElapsed = 0f;
        Color startColor = lr.startColor;
        Color endColor = lr.endColor;

        while (fadeElapsed < fadeTime)
        {
            fadeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, fadeElapsed / fadeTime);
            Color fadedStart = new Color(startColor.r, startColor.g, startColor.b, alpha);
            Color fadedEnd = new Color(endColor.r, endColor.g, endColor.b, alpha);
            lr.startColor = fadedStart;
            lr.endColor = fadedEnd;
            yield return null;
        }

        Destroy(beam);
    }

    
}
