using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    public bool killEverything = false;
    public string source;

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
        if ((collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<Player>().invincible == false) || (killEverything && !collision.CompareTag("Indestructable")))
        {
            // ... Destroy the object that collided with me
            if (collision.gameObject.CompareTag("Player"))
                GameObject.Find("GameManager").GetComponent<GameManager>().GameOver(source);
            Destroy(collision.gameObject);
        }
    }
}
