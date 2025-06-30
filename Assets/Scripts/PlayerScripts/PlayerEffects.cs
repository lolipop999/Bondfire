using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    public ParticleSystem speedBoostParticles;
    public ParticleSystem healthBoostParticles;
    private SpriteRenderer playerRenderer;

    void Start()
    {
        playerRenderer = GetComponent<SpriteRenderer>();
    }

    public void EnableSpeedBoostTrail(bool enable)
    {
        if (enable)
        {
            speedBoostParticles.GetComponent<Renderer>().sortingOrder = playerRenderer.sortingOrder-1;
            speedBoostParticles.Play();
        }
        else
        {
            speedBoostParticles.Stop();
        }
    }

    public void EnableHealthBoostParticles()
    {
        healthBoostParticles.GetComponent<Renderer>().sortingOrder = playerRenderer.sortingOrder-1;
        healthBoostParticles.Play();
    }
     


}
