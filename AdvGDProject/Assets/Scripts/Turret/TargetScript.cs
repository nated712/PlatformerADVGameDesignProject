using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;
    private PlayerHealthManager phm;

    [SerializeField] private GameObject brokenTurretPrefab; // Assign in Inspector
    [SerializeField] private GameObject smokeEffectPrefab;  // Assign in Inspector
    [SerializeField] private Transform turretHead;          // Assign in Inspector

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player"); 
        if (player != null)
        {
            phm = player.GetComponent<PlayerHealthManager>();
        }

        if (phm == null)
        {
            Debug.LogError("PlayerHealthManager not found on Player object!");
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        // Heal the player on kill
        if (phm != null)
        {
            phm.TakeDamage(-300);
        }

        // Store the turret head rotation
        Quaternion headRotation = turretHead.rotation;

        // Spawn broken turret
        GameObject brokenTurret = null;
        if (brokenTurretPrefab != null)
        {
            brokenTurret = Instantiate(brokenTurretPrefab, transform.position, transform.rotation);
        }



        // Reattach turret head to the broken turret (or leave it floating)
        if (turretHead != null)
        {
            turretHead.SetParent(null); // Detach from original turret body
            turretHead.rotation = headRotation; // Keep its rotation

            if (brokenTurret != null)
            {
                turretHead.SetParent(brokenTurret.transform); // Optional: Attach to broken turret
            }
        }

        // Spawn smoke effect
        if (smokeEffectPrefab != null)
        {
            GameObject smoke = Instantiate(smokeEffectPrefab, turretHead.position, Quaternion.identity);
            smoke.transform.SetParent(null); // Detach smoke from broken turret
        }

        // Destroy the original turret body
        Destroy(gameObject);
    }
}
