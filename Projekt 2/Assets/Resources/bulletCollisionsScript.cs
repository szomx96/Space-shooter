using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletCollisionsScript : MonoBehaviour
{
    private GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        explosionPrefab = Resources.Load("Explosion") as GameObject;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.gameObject.CompareTag("Bullet"))
            explode(collision);
    }

    public void explode(Collision collision)
    {
        GameObject explosion = Instantiate(explosionPrefab) as GameObject;
        explosion.transform.position = transform.position;
        Destroy(gameObject);
        Destroy(explosion, 5.0f);
    }
}
