using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    [Header("Player")]
    public GameObject Player;
    public GameObject Fuel;
    public bool followPlayer = true;
    public bool followFuel = false;
    private Vector3 playerPos;
    private Vector3 fuelPos;

    [Header("Zoom")]
    // How much the camera should zoom out
    private float zoom;
    // How heavily speed impacts zoom
    public float zoomScale;
    // The baseline zoom before the player moves
    [SerializeField] private float zoomBase = 6.5f;

    // Start is called before the first frame update
    void Start()
    {
        if (Player == null)
        {
            Player = GameObject.Find("Player");
        }
        if (Fuel == null)
        {
            Fuel = GameObject.Find("Fuel");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If I should be following the player...
        if (followPlayer && Player)
        {
            // ... Follow the player
            playerPos = Player.transform.position;
            gameObject.transform.position = new Vector3(playerPos.x, playerPos.y, -1);
            // ... Set the zoom based on player speed
            zoom = Player.GetComponent<Rigidbody2D>().velocity.magnitude * zoomScale;
            GetComponent<Camera>().orthographicSize = zoomBase + zoom;
        }

        // If I should be following the fuel...
        if (!followPlayer && followFuel)
        {
            // ... Follow the fuel
            fuelPos = Fuel.transform.position;
            gameObject.transform.position = new Vector3(fuelPos.x, fuelPos.y, -1);
        }
    }
}
