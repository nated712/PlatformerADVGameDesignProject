using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Timers;

public class GunControl : MonoBehaviour
{
    public Transform gunBarrell;
    public TrailRenderer bulletTrail;

    
    public float damage = 30f;
    public float range = 100f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;

    public Image hitMarker;
    public float hitmarkerDuration = 0.15f;

    void Start()
    {
        hitMarker.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        var bullet = Instantiate(bulletTrail, gunBarrell.position, Quaternion.identity);
        bullet.AddPosition(gunBarrell.position);
        {
            bullet.transform.position = transform.position + (fpsCam.transform.forward * 200);
        }
        muzzleFlash.Play();
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
                StartCoroutine(ActivateHitMarker());
            }
        }
    }

    IEnumerator ActivateHitMarker()
    {
        hitMarker.enabled = true;
        yield return new WaitForSeconds(hitmarkerDuration);
        hitMarker.enabled = false;
    }
}
