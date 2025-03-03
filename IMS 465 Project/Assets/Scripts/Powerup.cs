using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private Player player;
    private Storage storage;

    public int id;
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        storage = GameObject.Find("Storage").GetComponent<Storage>();
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
            storage.HoldPowerUp(id);
            Destroy(gameObject);
        }
    }
}
