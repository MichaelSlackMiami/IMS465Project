using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [Header("Grapple Properties")]
    public float maxDistance;
    public float pullStrength;
    public float timeToExtend;
    public float timeToRetract;
    public float waitTime;
    public bool grappleDisabled = false;

    private Vector2 force;

    [Header("Hook")]
    public GameObject hook;

    private GameObject myHook;


    [Header("Line")]
    public LineRenderer lineR;
    public bool lineOut;

    private Vector3 grappleVector;
    private float currentGrappleLength;
    private float currentRaycastLength;
    private Coroutine sendingOut;
    private Coroutine bringingBack;


    [Header("Physics")]
    private Vector2 initialDirection;
    private Vector2 hookDirection;
    private Vector2 swingForce;
    private Vector2 relativeVelocity;
    private float velocityAlongHook;
    private Vector2 radius;
    private float angularVelocityRad;
    private Vector2 rotationalVelocity;
    private Vector2 hookVelocity;
    private float systemMass;


    [Header("Player")]
    public Rigidbody2D player;

    private Vector2 playerPosition;
    private Vector2 mouse;


    [Header("Object")]
    private RaycastHit2D hit;
    private bool isFreeBody;

    [Header("Debug")]
    public GameObject rangeIndicator;

    void Start()
    {
        // Set the debug range indicator's size
        rangeIndicator.transform.localScale *= (2 * maxDistance);

        currentGrappleLength = maxDistance;
    }

    void Update()
    {
        // Convert position to 2d
        playerPosition = transform.position;
        if (!grappleDisabled)
        {
            if (Input.GetMouseButtonDown(0)) // left click
            {
                // Get mouse position and direction from Player
                mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                initialDirection = (mouse - playerPosition).normalized;

                // Reinstate Line Renderer and set start to Player
                lineOut = true;
                lineR.positionCount = 2;

                if (bringingBack != null)
                    StopCoroutine(bringingBack);
                sendingOut = StartCoroutine(SendOutHook(timeToExtend));
            }

            if (Input.GetMouseButton(0)) // Left click held
            {
                if (myHook)
                {
                    // Get the direction of the line
                    hookDirection = (myHook.transform.position - player.transform.position).normalized;

                    // Calculate the grapple force
                    force = hookDirection * pullStrength * Time.deltaTime;

                    // Pull the player
                    player.AddForce(force);


                    if (hit)
                    {
                        // Pull the grappled object
                        hit.rigidbody.AddForceAtPosition(-force, myHook.transform.position);

                        // Update the grapple length
                        grappleVector = lineR.GetPosition(0) - lineR.GetPosition(1);

                        // Check if grapple is exceeding its max length
                        if (grappleVector.magnitude > currentGrappleLength)
                        {
                            SwingOnLine();
                        }

                        currentGrappleLength = grappleVector.magnitude;
                        initialDirection = hookDirection;
                    }
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

            // Retract line
            if (lineOut)
            {
                // Cancel sending
                if (sendingOut != null)
                    StopCoroutine(sendingOut);

                // Start bringing back
                bringingBack = StartCoroutine(BringBackHook(timeToRetract));
            }
        }

        if (lineOut)
        {
            UpdateLine();
        }
    }

    IEnumerator SendOutHook(float seconds)
    {
        // Set the line length to 0
        currentRaycastLength = 0;

        while (myHook == null && Input.GetMouseButton(0) && currentRaycastLength < maxDistance)
        {
            // Slowly increase length
            currentRaycastLength += (Time.deltaTime * maxDistance / seconds);
            if (currentRaycastLength > maxDistance)
            {
                currentRaycastLength = maxDistance;
            }

            // Send out hook based on this length
            ShootHook(currentRaycastLength);

            yield return new WaitForEndOfFrame();
        }

        // Wait to avoid instant out and back
        yield return new WaitForSeconds(waitTime);

        // Check if hit during wait time
        if (myHook == null)
        {
            ShootHook(currentRaycastLength);
        }

        // If no hit, bring back hook
        if (currentRaycastLength == maxDistance && myHook == null)
        {
            StartCoroutine(BringBackHook(timeToRetract));
        }
    }

    IEnumerator BringBackHook(float seconds)
    {
        // Incrementally decrease length
        while (currentRaycastLength > 0 && !Input.GetMouseButtonDown(0))
        {
            currentRaycastLength -= (Time.deltaTime * maxDistance / seconds);
            if (currentRaycastLength < 0)
            {
                currentRaycastLength = 0;
            }

            yield return new WaitForEndOfFrame();
        }

        // Destroy  line
        lineOut = false;
        lineR.positionCount = 0;
    }

    private void ShootHook(float distance)
    {
        // Generate collision
        hit = Physics2D.Raycast(transform.position, initialDirection, distance);

        if (hit.collider) // if grappling hook hit something
        {
            // Make sure previous hook gets destroyed
            if (myHook)
            {
                Destroy(myHook);
            }

            // Create hook prefab at collision as child of object
            myHook = Instantiate(hook, hit.transform, true);
            myHook.transform.position = hit.point;
            myHook.transform.localRotation = Quaternion.Euler(0, 0, Vector2.Angle(Vector2.up, hit.normal));

            // Check if the hit object is free moving or not (useful later)
            isFreeBody = hit.rigidbody.constraints == RigidbodyConstraints2D.None;
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
            lineR.SetPosition(1, playerPosition + (initialDirection * currentRaycastLength));
        }
    }

    private void SwingOnLine()
    {
        // Find the velocity (rotational) of the hook relative the center of the hit object
        radius = myHook.transform.position - hit.transform.position;
        angularVelocityRad = hit.rigidbody.angularVelocity * Mathf.Deg2Rad;
        rotationalVelocity = new Vector2(-radius.y, radius.x) * angularVelocityRad;

        // Get the hook's total velocity by add its relative velocity to the object's velocity
        hookVelocity = hit.rigidbody.velocity + rotationalVelocity;

        // Calculate the velocity along the line (lengthening/shortening speed)
        relativeVelocity = player.velocity - hookVelocity;
        velocityAlongHook = Vector2.Dot(relativeVelocity, hookDirection);

        // To counter lengthening, we need the opposite of that velocity
        swingForce = -hookDirection * velocityAlongHook;

        if (isFreeBody) // Check if the other object will be affected by the swing
        {
            // Get the mass of the system (The more inertia of the system, the more force needed to balance it)
            systemMass = (hit.rigidbody.mass + player.mass);
        }
        else
        {
            // Just get the player if the object is stationary
            systemMass = player.mass;
        }

        swingForce *= systemMass;

        // Add the swing force
        player.AddForce(swingForce);

        if (hit && isFreeBody) // Check if should add force to object
        {
            // Add the swing force
            hit.rigidbody.AddForceAtPosition(-swingForce, myHook.transform.position);
        }
    }

    private void OnDisable()
    {
        if (myHook)
        {
            Destroy(myHook);
        }
    }
}
