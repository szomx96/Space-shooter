using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public int bulletVelocity = 65;

    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    public GameObject laserBolt;


    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = Resources.Load("bullet") as GameObject;
        explosionPrefab = Resources.Load("Explosion") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject bullet = Instantiate(bulletPrefab) as GameObject;
            bullet.transform.position = transform.position + Camera.main.transform.forward;
            Physics.IgnoreCollision(bullet.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = Camera.main.transform.forward * bulletVelocity;
            Destroy(bullet, 5.0f);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject lb = Instantiate(laserBolt, Camera.main.transform.position, Camera.main.transform.rotation);
            Physics.IgnoreCollision(lb.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
            Destroy(lb, 3f);
        }
    }

    public void gotHit(Collision collision)
    {
        GameObject explosion = Instantiate(explosionPrefab) as GameObject;
        explosion.transform.position = collision.gameObject.transform.position;
        Destroy(collision.gameObject);
        Destroy(explosion, 5.0f);
    }
}
