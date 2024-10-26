using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    public bool killEverything = false;

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
        // If the player collides with this object...
        if (collision.gameObject.CompareTag("Player") || killEverything)
        {
            // ... Kill the player
            Destroy(collision.gameObject);
        }
    }
}
