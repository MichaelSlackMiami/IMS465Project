using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAtPlayer : MonoBehaviour
{
    public ShootSpawner shooter;
    private Transform player;
    public bool inRange;

    private float magnitude;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        magnitude = shooter.velocity.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = shooter.velocity.normalized;

        if (player)
            direction = (player.transform.position - gameObject.transform.position).normalized;

        shooter.velocity = direction * magnitude;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = true;
            shooter.isSpawning = inRange;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = false;
            shooter.isSpawning = inRange;
        }
    }
}
