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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the player collides with me OR if I should destroy everything...
        if (collision.gameObject.CompareTag("Player") || killEverything)
        {
            // ... Destroy the object that collided with me
            Destroy(collision.gameObject);
        }
    }
}
