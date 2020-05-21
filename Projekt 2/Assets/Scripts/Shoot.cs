using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shoot : MonoBehaviour
{

    public GameObject bullet;
    public GameObject laserBolt;
    public float distance = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Ray point = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit rcHit;

            if(Physics.Raycast(point, out rcHit, Mathf.Infinity))
            {
                bullet.transform.position = rcHit.point;
            }

            //Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
            ////position = Camera.main.ScreenToWorldPoint(position);
            //GameObject go = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
            //go.transform.LookAt(position);
            //Debug.Log(position);
            //go.GetComponent<Rigidbody>().AddForce(go.transform.forward * 10000);
        }

        if (Input.GetKey(KeyCode.F))
        {
            Instantiate(laserBolt, gameObject.transform.position, gameObject.transform.rotation);
        }
    }
}
