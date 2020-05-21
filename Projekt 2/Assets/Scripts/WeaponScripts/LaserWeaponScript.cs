using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeaponScript : MonoBehaviour, IWeapon
{
    public GameObject laserBolt;
    public float fireRate = 0.3f;

    private float tRate = 0f;

    public void Shoot()
    {
        if (Time.time > tRate)
        {
            tRate = Time.time + fireRate;
            GameObject lb = Instantiate(laserBolt) as GameObject;
            lb.transform.position = gameObject.transform.position;
            lb.transform.rotation = gameObject.transform.rotation;
            //ignore collision between this laserbolt and any laserbolt?
            Physics.IgnoreCollision(lb.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
            Destroy(lb, 5.0f);
        }
    }
}
