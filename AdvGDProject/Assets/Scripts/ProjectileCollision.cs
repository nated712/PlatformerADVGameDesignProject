using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
//    public GameObject impactEffect;

    public float radius = 3;
    public int damageAmount = 15;


    private void OnTriggerEnter(Collider other)
    {
        //GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
        //Destroy(impact, 2);
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealthManager>().TakeDamage(damageAmount);
            Destroy(gameObject);
        }
        
    }
}
