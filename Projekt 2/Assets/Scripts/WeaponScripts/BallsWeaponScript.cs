using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsWeaponScript : MonoBehaviour, IWeapon
{

    public int bulletVelocity = 90;
    public float fireRate = 10f;
    public float spread = 0.1f;
    public int nPellets = 10;

    private GameObject bulletPrefab;
    private GameObject explosionPrefab;
    private float tRate = 0f;
    private Rigidbody parentRb;
    private int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = Resources.Load("bullet") as GameObject;
        explosionPrefab = Resources.Load("Explosion") as GameObject;
        parentRb = gameObject.GetComponent<Rigidbody>();
    }

    //niech pociski eksplouja po czasie albo w poblizu przeciwnika albo na zderzeniu

    public void Shoot()
    {
        if (Time.time > tRate)
        {
            tRate = Time.time + fireRate;
            for (i = 0; i < nPellets; i++)
            {
                Vector3 randomSpread = new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), Random.Range(-spread, spread));
                GameObject bullet = Instantiate(bulletPrefab) as GameObject; //przepisac na queue
                bullet.transform.position = transform.position + randomSpread;
                //spread sie naprawi implenetujac object pool
                Physics.IgnoreCollision(bullet.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                rb.rotation = parentRb.rotation;
                //rb.rotation = new Quaternion(parentRb.rotation.x * Random.Range(-spread, spread), parentRb.rotation.y * Random.Range(-spread, spread), parentRb.rotation.z * Random.Range(-spread, spread), parentRb.rotation.w * Random.Range(-spread, spread)); //jezu
                rb.AddRelativeForce(parentRb.velocity +(Vector3.forward + randomSpread) * bulletVelocity, ForceMode.Impulse);
                //rb.velocity = parentRb.velocity + (Vector3.forward * bulletVelocity);
                Destroy(bullet, 5.0f);
            }  
        }
    }

}
