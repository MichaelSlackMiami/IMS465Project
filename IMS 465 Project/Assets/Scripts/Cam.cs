using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    [Header("Player")]
    public GameObject Player;
    public bool followPlayer = true;
    private Vector3 playerPos;

    [Header("Zoom")]
    // How much the camera should zoom out
    private float zoom;
    // How heavily speed impacts zoom
    public float zoomScale;
    // The baseline zoom before the player moves
    private float zoomBase;

    // Start is called before the first frame update
    void Start()
    {
        zoomBase = GetComponent<Camera>().orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        // If I should be following the player...
        if (followPlayer)
        {
            // ... Follow the player
            playerPos = Player.transform.position;
            gameObject.transform.position = new Vector3(playerPos.x, playerPos.y, -1);
            // ... Set the zoom based on player speed
            zoom = Player.GetComponent<Rigidbody2D>().velocity.magnitude * zoomScale;
            GetComponent<Camera>().orthographicSize = zoomBase + zoom;
        }
    }
}
