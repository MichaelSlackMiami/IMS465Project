using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [Header("Grapple Properties")]
    public float distance;
    public float pullStrength;

    private Vector2 force;

    [Header("Hook")]
    public GameObject hook;

    private GameObject myHook;


    [Header("Line")]
    public LineRenderer lineR;
    public bool lineOut;

    private Vector2 direction;


    [Header("Player")]
    public Rigidbody2D player;
    private Vector2 myPosition;
    private Vector2 mouse;


    [Header("Object")]
    private RaycastHit2D hit;

    [Header("Debug")]
    public GameObject rangeIndicator;

    void Start()
    {
        // Set the debug range indicator's size
        rangeIndicator.transform.localScale *= (2 * distance);
    }

    void Update()
    {
        // Convert position to 2d
        myPosition = transform.position;

        if (Input.GetMouseButtonDown(0)) // left click
        {
            // Get mouse position and direction from Player
            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = (mouse - myPosition).normalized;

            // Generate collision
            hit = Physics2D.Raycast(transform.position, direction, distance);

            // Reinstate Line Renderer and set start to Player
            lineOut = true;
            lineR.positionCount = 2;

            if (hit.collider) // if grappling hook hit something
            {
                // Create hook prefab at collision as child of object
                myHook = Instantiate(hook, hit.transform, true);
                myHook.transform.position = hit.point;
                myHook.transform.localRotation = Quaternion.Euler(0, 0, Vector2.Angle(Vector2.up, hit.normal));
            }
        }

        if (Input.GetMouseButton(0)) // Left click held
        {
            if (myHook)
            {
                // Calculate the grapple force
                force = (myHook.transform.position - player.transform.position).normalized * pullStrength * Time.deltaTime;

                // Pull the player
                player.AddForce(force);

                // Pull the grappled object
                if (hit)
                {
                    hit.rigidbody.AddForceAtPosition(-force, myHook.transform.position);
                }
            }
        }


        if (Input.GetMouseButtonUp(0)) // left click
        {
            // Destroy hook
            if (myHook)
            {
                Destroy(myHook);
            }

            // Destroy  line
            lineOut = false;
            lineR.positionCount = 0;
        }

        if (lineOut)
        {
            UpdateLine();
        }
    }

    private void UpdateLine()
    {
        // Anchor the first point on the player
        lineR.SetPosition(0, transform.position);

        if (myHook)
        {
            // Set the second point to the hook
            lineR.SetPosition(1, myHook.transform.position);
        }
        else
        {
            // Set the second point to the missed location
            lineR.SetPosition(1, myPosition + (direction * distance));
        }
    }
}
