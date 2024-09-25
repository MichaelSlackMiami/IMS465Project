using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [Header("Grapple Properties")]
    public float distance;

    [Header("Hook")]
    public GameObject hook;

    private GameObject myHook;

    [Header("Line")]
    public LineRenderer lineR;
    public bool lineOut;

    private Vector2 direction;

    [Header("Player")]
    public Rigidbody2D player;

    private Vector2 playerPosition2D;
    private Vector2 mouse;
    private Vector2 targetVector;

    void Update()
    {
        // Get location in 2d
        playerPosition2D = new Vector2(transform.position.x, transform.position.y);

        if (Input.GetMouseButtonDown(0)) // left click
        {

            // Get mouse position and direction from Player
            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = mouse - playerPosition2D;


            // Generate collision
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance);

            Debug.Log(hit.collider);

            // Reinstate Line Renderer and set start to Player
            lineOut = true;
            lineR.positionCount = 2;
            lineR.SetPosition(0, playerPosition2D);

            if (hit.collider) // if grappling hook hit something
            {
                // Create hook prefab at collision as child of object
                myHook = Instantiate(hook, hit.transform, true);
                myHook.transform.position = hit.point;
                myHook.transform.localRotation = Quaternion.Euler(0, 0, Vector2.Angle(Vector2.up, hit.normal));

                // Set end of line
                lineR.SetPosition(1, hit.point);

                //Enable the method that pulls the player toward the hook
                //PullPlayer(true);
            }
            else // if grappling hook missed
            {
                // check if mouse is in range
                if (direction.magnitude > distance) 
                {
                    // travel as far as possible towards mouse
                    targetVector = direction.normalized * distance;
                }
                else
                {
                    // travel to mouse
                    targetVector = mouse - playerPosition2D;
                }
            }


        }


        if (Input.GetMouseButtonUp(0)) // left click
        {
            // Destroy hook and line
            if (myHook)
            {
                Destroy(myHook);
            }

            lineOut = false;
            lineR.positionCount = 0;

            //Disbale the line's pull on the player
            //PullPlayer(false);
        }

        // Pull player whenever hook is active
        PullPlayer(myHook, 3f);

        // Update line positions when hook is active
        if (lineOut)
        {
            // Line stays with the player
            lineR.SetPosition(0, transform.position);

            if (myHook)
            {
                // Line stays connected to hook
                lineR.SetPosition(1, myHook.transform.position);
            }
            else
            {
                // Moves the line with the player
                lineR.SetPosition(1, targetVector + playerPosition2D);
            }
        }
    }
    void PullPlayer(bool enabled, float scalar)
    {
        if (enabled == true)
        {
            player.AddForce(scalar * (myHook.transform.position - player.transform.position) * Time.deltaTime);
        }
    }
}
