using UnityEngine;

public class Player_Bow : MonoBehaviour
{
    public Transform arrowLaunchPoint;
    public GameObject arrowPrefab;
    private Vector2 aimDirection = Vector2.right;
    private float shootTimer;
    public Animator anim;
    public PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerMovement.isActive) return;
        shootTimer -= Time.deltaTime;
        HandleAiming();
        if (Input.GetButtonDown("ShootArrow") && shootTimer <= 0)
        {
            playerMovement.isShooting = true;
            anim.SetBool("isShooting", true);
        }
    }

    public void Shoot()
    {
        if (shootTimer <= 0)
        {
            Arrow arrow = Instantiate(arrowPrefab, arrowLaunchPoint.position, Quaternion.identity).GetComponent<Arrow>();
            arrow.direction = aimDirection;
            
            shootTimer = StatsManager.Instance.shootCooldown;
        }
        anim.SetBool("isShooting", false);
        playerMovement.isShooting = false;
    }

    private void HandleAiming()
    {
        // Get mouse position in world space
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // Get direction from player to mouse
        Vector2 direction = (mouseWorldPos - transform.position).normalized;

        // Set aim direction
        aimDirection = direction;

        // Flip player direction based on horizontal aim
        if (aimDirection.x < 0 && playerMovement.facingDirection == 1)
        {
            playerMovement.flip(); // now facing left
        }
        else if (aimDirection.x > 0 && playerMovement.facingDirection == -1)
        {
            playerMovement.flip(); // now facing right
        }

        anim.SetFloat("aimX", aimDirection.x);
        anim.SetFloat("aimY", aimDirection.y);
        
    }
    private void OnEnable()
    {
        anim.SetLayerWeight(1, 1);
    }
    private void OnDisable()
    {
        anim.SetLayerWeight(1, 0);
    }
}
