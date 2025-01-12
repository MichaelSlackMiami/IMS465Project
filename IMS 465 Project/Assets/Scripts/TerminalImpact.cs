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
            // Get the impact vector
            Vector2 impact = collision.relativeVelocity;

            // Percent of damage is affected by angle to hit (0 = just speed, 1 = fully based on angle)
            float glanceRatio = 0.5f;

            // Formula for death, not completely linear
            float impactStrength = (impact.magnitude * (1 - glanceRatio)) + (impact.magnitude * Vector2.Dot(collision.GetContact(0).normal, impact.normalized) * glanceRatio);

            // ... If the impact has enough force...
            if (impactStrength > terminalImpactThreshold)
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
