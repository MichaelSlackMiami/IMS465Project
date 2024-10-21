using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float terminalImpactThreshold;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the impact has enough force... then kill the player
        if (collision.relativeVelocity.magnitude > terminalImpactThreshold)
        {
            DeathSequence();
        }
    }

    public void DeathSequence()
    {
        // Kill the player
        Debug.Log("You died.");
    }
}
