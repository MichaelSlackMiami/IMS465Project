using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // When the Player collides with this object...
        if (collision.gameObject.CompareTag("Player"))
        {
            // ... Trigger victory sequence
            Debug.Log("You win!");
        }
    }
}
