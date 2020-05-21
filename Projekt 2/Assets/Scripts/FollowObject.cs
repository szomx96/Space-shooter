using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public GameObject gObject;
    private Vector3 offset = new Vector3(0,1,-3);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetPositionAndRotation(gObject.transform.position, gObject.transform.rotation);
        transform.Translate(offset);
        

        //transform.Translate(offset, gObject)
    }
}
