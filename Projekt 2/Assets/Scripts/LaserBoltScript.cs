using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBoltScript : MonoBehaviour
{
    public float speed = 10f;
    public float dmg = 1f;
    // Start is called before the first frame update
    void Start()
    {
        //gameObject.GetComponent<Rigidbody>().velocity = Vector3.forward * speed;

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //EnemyPlayer xD = collision as EnemyPlayer;
            Destroy(gameObject);
            collision.gameObject.GetComponent<EnemyPlayer>().getDamage(dmg);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            //EnemyPlayer xD = collision as EnemyPlayer;
            Destroy(gameObject);
            //collision.gameObject.GetComponent<PlayerController>().getDamage(dmg);
        }

        Destroy(gameObject);
    }

}
