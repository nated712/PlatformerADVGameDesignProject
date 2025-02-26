using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
//    public GameObject impactEffect;

    public float radius = 3;
    public int damageAmount = 15;

    private void Start()
    {
        // Destroy the projectile after 5 seconds
        Destroy(gameObject, 5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealthManager playerHealth = other.GetComponent<PlayerHealthManager>();
            playerHealth.TakeDamage(damageAmount);
            Destroy(gameObject);
        }
        
    }
}
