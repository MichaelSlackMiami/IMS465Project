using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSpawner : MonoBehaviour
{
    public GameObject projectile;

    public Vector2 velocity;

    public float waitTime = 5.0f;

    public bool spawnImmediately;
    public bool isSpawning = true;

    private float timer = 0.0f;
    private GameObject justSpawned;

    // Start is called before the first frame update
    void Start()
    {
        if (spawnImmediately)
        {
            Spawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > waitTime)
        {
            if (isSpawning)
                Spawn();
            timer = 0.0f;
        }
    }

    public void Spawn()
    {
        justSpawned = Instantiate(projectile, transform.position, Quaternion.identity);
        if (justSpawned.GetComponent<Rigidbody2D>())
        {
            justSpawned.GetComponent<Rigidbody2D>().velocity = velocity;
        }
    }
}
