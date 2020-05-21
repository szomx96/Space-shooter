using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Range(0.1f,1f)]
    public float engageRange = 1f; //make it percent of collider's radius
    private float _engageRange;
    [Range(0.1f, 1f)]
    public float evadeRange = 0.5f;
    public float evadeCd = 10f; // in seconds?

    private IWeapon weapon;
    private SphereCollider sphereCollider;
    private Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        weapon = GetComponent<IWeapon>();
        rigidbody = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        engageRange = engageRange * sphereCollider.radius * 5; //nie dziala jak powinnos to * 3 to narazie bandaid
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        float distance = Vector3.Distance(other.transform.position, transform.position);

        if (distance > engageRange && other.CompareTag("Player"))
        {
            followPlayer(other);
        }
        else if (distance <= engageRange && other.CompareTag("Player"))
        {
            engagePlayer(other);
        }

        if (distance < evadeRange && other.CompareTag("Projectile")) { }
            //evadeProjectile();
           
        
    }

    void engagePlayer(Collider other)
    {
        // jest juz ustawiony w kierunku gracza, trzeba wycelowac tam gdzie bedzie a nie gdzie jest
        lookAtTarget(other);
        //strzelic
        weapon.Shoot();
    }

    private void followPlayer(Collider other)
    {
        lookAtTarget(other);
        rigidbody.AddRelativeForce(Vector3.forward * 10f, ForceMode.Impulse);
    }

    void evadeProjectile()
    {
        transform.Translate(Vector3.left * 10 * Time.deltaTime);
    }

    private void lookAtTarget(Collider other)
    {
        Quaternion targetRotation = Quaternion.LookRotation(other.transform.position - transform.position);
        targetRotation = Quaternion.Slerp(transform.rotation, targetRotation, 5.0f * Time.deltaTime);
        rigidbody.rotation = targetRotation;
    }
}
