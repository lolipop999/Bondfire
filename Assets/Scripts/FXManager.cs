using Unity.VisualScripting;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    public static FXManager Instance;

    [Header("Audio Clips")]
    public AudioClip swordHitEnemy;
    public AudioClip swordSwing;
    public AudioClip enemyHitPlayer;
    public AudioClip arrowRelease;
    public AudioClip arrowHit;
    public AudioClip levelUp;
    public AudioClip shopPurchase;
    public AudioClip openStats;
    public AudioClip changeEquipment;
    public AudioClip heal;
    public AudioClip wizardZap;
    public AudioClip rockSmash;

    private AudioSource audioSource;
    void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip, volume);
    }

    public void PlayParticles(GameObject prefab, Vector3 position)
    {
        if (prefab != null)
            Instantiate(prefab, position, Quaternion.identity);
    }
}
