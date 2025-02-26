using System.Collections;
using UnityEngine;

public class TurretControl : MonoBehaviour
{
    Transform _Player;
    float dist; //Distance from player
    public float maxDistance; //How close the player needs to be to the turret for it to become active.
    public Transform head, barrel;
    public GameObject _projectile;
    public float fireRate, nextFire;
    public float shotSpeed;

    void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    void Update() //Moves the barel and box of the turret to player position when in range.
    {
        dist = Vector3.Distance(_Player.position, transform.position);
        if (dist <= maxDistance)
        {
            head.LookAt(_Player);
            if(Time.time >= nextFire)
            {
                nextFire = Time.time + 1f /fireRate;
                shoot();
            }
           
        }
    }

    void shoot() //Clones whatever the projectile is set to, assumes Rigidbody, and adds force in the direction of the barrel.
    {
        GameObject clone = Instantiate(_projectile, barrel.position, head.rotation);
        clone.GetComponent<Rigidbody>().AddForce(head.forward * shotSpeed);
        Destroy(clone, 10);
    }
}
