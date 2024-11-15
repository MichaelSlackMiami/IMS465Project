using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel : MonoBehaviour
{
    [SerializeField] private GameManager GameManager;
    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
            // ... Alert the Game Manager that the player has won
            GameManager.LevelClear();

            // Destroy self for visual feedback
            // Destroy(gameObject);
        }
    }
}
