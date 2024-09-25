using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [Header("Grapple Properties")]
    public float distance;
    public float pullStrength;
    public float rotationalStrength;


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

    void Update()
    {
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

                //Rotate whatever hook hit
                hit.rigidbody.AddTorque(rotationalStrength, ForceMode2D.Impulse);
            }
        }

        if (Input.GetMouseButton(0)) // Left click held
        {
            if (myHook)
            {
                //Pull the player
                player.AddForce((myHook.transform.position - player.transform.position) * pullStrength);
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
        }

        if (lineOut)
        {
            UpdateLine();
        }
    }

    private void UpdateLine()
    {
        lineR.SetPosition(0, transform.position);
        Debug.Log(myHook);
        Debug.Log(lineOut);

        if (myHook)
        {
            lineR.SetPosition(1, myHook.transform.position);
        }
        else
        {
            lineR.SetPosition(1, myPosition + (direction * distance));
        }
    }
}
