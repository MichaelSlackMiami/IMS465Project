using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [Header("Grapple Properties")]
    public float maxDistance;
    public float pullStrength;
    public float pullStengthCap;
    public float ratchetRate;
    public float timeToExtend;
    public float timeToRetract;
    public float waitTime;
    public bool grappleDisabled = false;

    private Vector2 force;
    public float currentPullStrength;

    [Header("Hook")]
    public GameObject hook;

    private GameObject myHook;
    private GameObject unhitHook;

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

    [Header("GameManager")]
    [SerializeField] private GameManager GM;


    [Header("Object")]
    private RaycastHit2D hit;
    private bool isFreeBody;


    [Header("Throwing")]
    public float heldTimer;
    public float heldTime;
    public bool canThrow;
    public float throwForce;
    public float aimForce;
    public float ogAngularDrag;
    public float aimAngularDrag;
    public GameObject canThrowVFX;

    [Header("Audio")]
    [SerializeField] private AudioSource GrappleFire;
    [SerializeField] private AudioSource GrappleConnect;

    [Header("Debug")]
    public GameObject rangeIndicator;

    void Start()
    {
        // Set the debug range indicator's size
        rangeIndicator.transform.localScale *= (2 * maxDistance);
        currentGrappleLength = maxDistance;

        // Connect to the GameManager
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Set grapple strength
        currentPullStrength = pullStrength;
    }

    void Update()
    {
        // Convert position to 2d
        playerPosition = transform.position;

        // Ratchet grapple strength
        if (myHook)
        {
            if (currentPullStrength < pullStengthCap)
                currentPullStrength += ratchetRate * Time.deltaTime;
        }
        else if (currentPullStrength != pullStrength)
        {
            currentPullStrength = pullStrength;
        }

        // Inputs
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

                // Create a hook for end of line
                if (unhitHook)
                    Destroy(unhitHook);
                unhitHook = Instantiate(hook);

                // Play SFX
                GrappleFire.Play();

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
                    force = hookDirection * currentPullStrength * Time.deltaTime;

                    // Pull the player
                    player.AddForce(force);


                    if (hit)
                    {
                        /*
                        // If the grappled object is the Fuel...
                        if (hit.rigidbody.gameObject.CompareTag("Fuel"))
                        {
                            // ... Clear the level
                            GM.LevelClear();
                        } else
                        */
                        {
                            // Pull the grappled object
                            hit.rigidbody.AddForceAtPosition(-force, myHook.transform.position);

                            // Update the grapple length
                            grappleVector = (Vector3)playerPosition - myHook.transform.position;

                            // Check if grapple is exceeding its max length
                            if (grappleVector.magnitude > currentGrappleLength)
                            {
                                SwingOnLine();
                            }
                            currentGrappleLength = grappleVector.magnitude;
                            initialDirection = hookDirection;
                        }

                        // This block of code will be used to rotate light objects
                        // around the player for the sake of aiming them
                        if (canThrow)
                        {
                            // Get mouse position and direction from Player
                            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            Vector2 inputDirection = (mouse - playerPosition).normalized;

                            // Determine direction of rotation with cross product
                            float crossProduct = (hookDirection.x * inputDirection.y) - (hookDirection.y * inputDirection.x);
                            int directionFlipper;

                            if (crossProduct > 0) // Need to rotate Counterclockwise
                            {
                                directionFlipper = 1;
                            }
                            else // Need to rotate Clockwise
                            {
                                directionFlipper = -1;
                            }

                            
                            // Determine how much it should be adjusted (more force if more off target)
                            float dot = Vector2.Dot(hookDirection.normalized, inputDirection.normalized); // range of (-1, 1)
                            dot *= -0.5f; // range of (0.5, -0.5)
                            dot += 1.5f; // range of (2, 1)... which means double force for furthest away, regular for correct

                            /*
                            // Apply a tangent force to the grappled object to aim it around the player
                            hit.rigidbody.AddForce(aimForce * Vector2.Perpendicular(hookDirection.normalized) * directionFlipper * Time.deltaTime);
                            */

                            // Apply a torque force to the player to rotate the whole system
                            player.AddTorque(aimForce * directionFlipper * dot * Time.deltaTime);
                        }
                    }
                }
            }
        }


        if (Input.GetMouseButtonUp(0)) // left click
        {
            TryThrowObject();

            RetractArm();
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

        // Destroy hook on end of line
        if (unhitHook)
        {
            Destroy(unhitHook);
        }
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

            // Destroy shooting hook
            if (unhitHook)
            {
                Destroy(unhitHook);
            }

            // Create hook prefab at collision as child of object
            myHook = Instantiate(hook, hit.transform, true);
            myHook.transform.position = hit.point;
            myHook.transform.up = hit.normal;
            myHook.layer = 2;

            // Play SFX
            GrappleConnect.Play();

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

            if (unhitHook)
            {
                // Set hook sprite to end of line
                unhitHook.transform.position = playerPosition + (initialDirection * currentRaycastLength);
                unhitHook.transform.up = -initialDirection;
            }
            else
            {
                unhitHook = Instantiate(hook);
                // Set hook sprite to end of line
                unhitHook.transform.position = playerPosition + (initialDirection * currentRaycastLength);
                unhitHook.transform.up = -initialDirection;
            }
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
            // Up the system mass enough to not get any stretch
            systemMass = 15 * player.mass;
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

        if (unhitHook)
            Destroy(unhitHook);
    }

    public void RetractArm()
    {
        // Destroy hook
        if (myHook)
        {
            // Update variables for bringing back line
            initialDirection = (myHook.transform.position - (Vector3)playerPosition).normalized;
            currentRaycastLength = (myHook.transform.position - (Vector3)playerPosition).magnitude;

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

        force = Vector2.zero;
        swingForce = Vector2.zero;
        currentGrappleLength = maxDistance;
    }

    public void TryThrowObject()
    {
        // This method is called everytime player unclicks

        // If can throw
        if (canThrow)
        {
            // Apply equal and opposite forces
            hit.rigidbody.AddForce(throwForce * hookDirection.normalized);
            player.AddForce(throwForce * -hookDirection.normalized);

            // Reset status
            canThrow = false;
            player.angularDrag = ogAngularDrag;
        }

        // Reset timer
        heldTime = 0;
    }

    public void CheckThrowable(GameObject other)
    {
        // If grabbed object is pulled all the way in and can't throw yet
        if (myHook && other == hit.collider.gameObject && !canThrow)
        {
            // Increase the held time for the sake of throwing
            heldTime += Time.deltaTime;

            // Once held long enough
            if (heldTime > heldTimer)
            {
                // Can throw
                canThrow = true;
                player.angularDrag = aimAngularDrag;
                Instantiate(canThrowVFX, myHook.transform.position, Quaternion.identity);

                // Reset timer
                heldTime = 0;
            }
        }
    }
}
