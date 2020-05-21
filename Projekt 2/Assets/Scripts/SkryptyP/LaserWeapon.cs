using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeapon : MonoBehaviour
{
    public GameObject laserBolt;
    public float fireRate = 10f;
    private float tRate = 0f;
    //TODO jakos ludzko to napisac
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Time.time > tRate)
            {
                tRate = Time.time + fireRate;
                GameObject lb = Instantiate(laserBolt) as GameObject;
                lb.transform.position = gameObject.transform.position;
                lb.transform.rotation = gameObject.transform.rotation;
                Physics.IgnoreCollision(lb.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
                gameObject.GetComponent<Collider>();
                Destroy(lb, 5.0f);
            }
        }
    }

    void Fire()
    {
        GameObject lb = Instantiate(laserBolt) as GameObject;
            lb.transform.position = gameObject.transform.position;
            lb.transform.rotation = gameObject.transform.rotation;
            Physics.IgnoreCollision(lb.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
            Destroy(lb, 5.0f);
    }
}
