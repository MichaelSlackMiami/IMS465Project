using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalImpact : MonoBehaviour
{
    public float terminalImpactThreshold = 4.0f;
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player collided with this object...
        if (collision.gameObject.CompareTag("Player"))
        {
            // ... If the impact has enough force...
            if (collision.relativeVelocity.magnitude > terminalImpactThreshold)
            {
                if (collision.gameObject.GetComponent<Player>().hasShield)
                {
                    collision.gameObject.GetComponent<Player>().RemoveShield();
                }
                else if (collision.gameObject.GetComponent<Player>().invincible == false)
                {
                    // ... Alert the GameManager that the player has died
                    GameManager.GameOver("Impact");
                }
            }
        }
    }
}
