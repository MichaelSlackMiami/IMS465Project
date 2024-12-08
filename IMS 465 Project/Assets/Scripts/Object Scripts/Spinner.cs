using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public Rigidbody2D myRB;
    [SerializeField] private float acceleration = 1.0f;
    [SerializeField] private float spinSpeed = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(myRB.angularVelocity) < spinSpeed)
        {
            myRB.AddTorque(acceleration * -1);
        }

    }
}
