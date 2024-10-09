using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [Header("Grapple Properties")]
    public float maxDistance;
    public float pullStrength;

    private Vector2 force;

    [Header("Hook")]
    public GameObject hook;

    private GameObject myHook;


    [Header("Line")]
    public LineRenderer lineR;
    public bool lineOut;

    private Vector2 initialDirection;
    private Vector2 hookDirection;
    private Vector2 swingForce;
    private Vector2 relativeVelocity;
    private float velocityAlongHook;

    [Header("Player")]
    public Rigidbody2D player;
    private Vector2 playerPosition;
    private Vector2 mouse;


    [Header("Object")]
    private RaycastHit2D hit;
    private float massRatio;
    bool isFreeBody;

    [Header("Debug")]
    public GameObject rangeIndicator;

    void Start()
    {
        // Set the debug range indicator's size
        rangeIndicator.transform.localScale *= (2 * maxDistance);
    }

    void Update()
    {
        // Convert position to 2d
        playerPosition = transform.position;

        if (Input.GetMouseButtonDown(0)) // left click
        {
            // Get mouse position and direction from Player
            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            initialDirection = (mouse - playerPosition).normalized;

            // Generate collision
            hit = Physics2D.Raycast(transform.position, initialDirection, maxDistance);

            // Reinstate Line Renderer and set start to Player
            lineOut = true;
            lineR.positionCount = 2;

            if (hit.collider) // if grappling hook hit something
            {
                // Create hook prefab at collision as child of object
                myHook = Instantiate(hook, hit.transform, true);
                myHook.transform.position = hit.point;
                myHook.transform.localRotation = Quaternion.Euler(0, 0, Vector2.Angle(Vector2.up, hit.normal));

                // Check if the hit object is free moving or not (useful later)
                isFreeBody = hit.rigidbody.constraints == RigidbodyConstraints2D.None;

            }
        }

        if (Input.GetMouseButton(0)) // Left click held
        {
            if (myHook)
            {
                // Get the direction of the line
                hookDirection = (myHook.transform.position - player.transform.position).normalized;

                // Calculate the grapple force
                force =  hookDirection * pullStrength * Time.deltaTime;

                // Pull the player
                player.AddForce(force);

                
                if (hit)
                {
                    // Pull the grappled object
                    hit.rigidbody.AddForceAtPosition(-force, myHook.transform.position);

                    // Balance system for fixed line length
                    SwingOnLine();
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
        lineR.SetPosition(0, playerPosition);

        if (myHook)
        {
            // Set the second point to the hook
            lineR.SetPosition(1, myHook.transform.position);
        }
        else
        {
            // Set the second point to the missed location
            lineR.SetPosition(1, playerPosition + (initialDirection * maxDistance));
        }
    }

    private void SwingOnLine()
    {
        relativeVelocity = player.velocity - hit.rigidbody.velocity;
        velocityAlongHook = Vector2.Dot(relativeVelocity, hookDirection);

        // Apply swing force only if the length wants to increase
        if (velocityAlongHook > 0)
        {
            swingForce = -hookDirection * velocityAlongHook;
        }
        else
        {
            swingForce = Vector2.zero; // No force if not lengthening
        }
       
        if (isFreeBody) // Check if the other object will be affected by the swing
        {
            // Determine the ratio to split the force along
            massRatio = hit.rigidbody.mass / (hit.rigidbody.mass + player.mass);
        }
        else
        {
            // If it is constrained, send all the force to the player
            massRatio = 1;
        }

        // Add the player's swing force
        player.AddForce(swingForce * massRatio);

        Debug.Log(swingForce.magnitude);

        if (hit && isFreeBody)
        {
            // Add the other object's swing force (constrained objects recieve the full force, but it shouldn't do anything)
            hit.rigidbody.AddForceAtPosition(-swingForce * (1 - massRatio), myHook.transform.position);
        }
    }
}
