using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class EnemyPlayer : MonoBehaviour
{
    public Image healthBar;

    //public float Hp { get; set; } = 100f;
    private float hp = 100;
    private float maxHp = 100;

    public float speed = 300.0f;
    public float damage = 10.0f;
    public float attackInterval = 1.0f;
    public float bulletAvoidRadius = 0.6f;
    public float bulletAvoidTime = 3.0f;

    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.name.Contains("bullet")) {

    //        Shooter shooter = GameObject.Find("FPSController").GetComponent<Shooter>();
    //        shooter.gotHit(collision);
    //        getDamage(20.0f);
    //    }
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.tag.Equals("Bullet"))
    //    {
    //        evadeProjectile();
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetType() == typeof(CapsuleCollider)) //nie dziala jak powinno
        {
            Debug.Log("Kapsula");
            if (other.CompareTag("Projectile"))
                evadeProjectile();
        }

        if (other.CompareTag("Player"))
        {

        }
    }

    private void engagePlayer()
    {

    }

    private void shoot()
    {

    }
    public void getDamage(float dmg)
    {
        hp -= dmg;
        healthBar.fillAmount = hp / maxHp;

        if (hp <= 0)
        {
            die();
        }
    }

    private void die()
    {
       //EnemyGenerator eg = GameObject.Find("FPSController").GetComponent<EnemyGenerator>();
        //eg.decrementEnemyCount();
        Destroy(gameObject);
    }

    private void followPlayer(Collider other)
    {
        Quaternion targetRotation = Quaternion.LookRotation(other.transform.position - transform.position);
        Quaternion.Slerp(transform.rotation, targetRotation, 5.0f * Time.deltaTime);
        transform.rotation = targetRotation;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (timer <= 0)
        {
            //other.SendMessage("GetDamage", damage);
            timer = attackInterval;
        }
        else
        {
            timer -= Time.deltaTime;
        }

    }

    private void evadeProjectile()
    {
        Vector3 destPosition = new Vector3(
            transform.position.x - Random.Range(-bulletAvoidRadius, bulletAvoidRadius),
            transform.position.y - Random.Range(-bulletAvoidRadius, bulletAvoidRadius),
            transform.position.z - Random.Range(-bulletAvoidRadius, bulletAvoidRadius));

        transform.position = Vector3.Lerp(transform.position, destPosition, bulletAvoidTime);
    }

}
