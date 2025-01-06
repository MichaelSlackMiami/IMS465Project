using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCam : MonoBehaviour
{
    private Vector3 boundaryPos;
    // Start is called before the first frame update
    void Start()
    {
        boundaryPos = GameObject.Find("Boundary").transform.position;
        gameObject.transform.position = new Vector3(boundaryPos.x, boundaryPos.y, -2.19f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
