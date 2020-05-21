using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator1 : MonoBehaviour
{
    public GameObject prefab;
    public float x = 100;
    public float y = 100;
    public float xStep = 2, yStep = 2;
    // Start is called before the first frame update
    void Start()
    {
        createStuff();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void createStuff()
    {
        int xD, yD;
        //plane.GetComponent<Renderer>().bounds.size;

        //narazie proste na plaszczyznie x,y z gory przyjetej
        // x i y zamienione na odwrót
        for (float i = -(x/2); i < x; i += xStep)
        {
            for (float j = -(y/2); j < y; j += yStep)
            {
                float yScale = Random.Range(2f, 40f);
                //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                GameObject @object = Instantiate(prefab);
                @object.transform.position = new Vector3(i, yScale/2, j);
                @object.transform.localScale = new Vector3(Random.Range(2f, 20f), yScale, Random.Range(2f, 20f));
            }
        }
    }
}
