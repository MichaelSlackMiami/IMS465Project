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

    private Vector2 direction;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // left click
        {
            // Get mouse position and direction from Player
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = mouse - transform.position;

            // Generate collision
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance);

            Debug.Log(hit.collider);

            // Reinstate Line Renderer and set start to Player
            lineR.positionCount = 2;
            lineR.SetPosition(0, transform.position);

            if (hit.collider) // if grappling hook hit something
            {
                // Create hook prefab at collision as child of object
                myHook = Instantiate(hook, hit.transform, true);
                myHook.transform.position = hit.point;
                myHook.transform.localRotation = Quaternion.Euler(0, 0, Vector2.Angle(Vector2.up, hit.normal));

                // Set end of line
                lineR.SetPosition(1, hit.point);
            }
            else // if grappling hook missed
            {
                // check if mouse is in range
                if (direction.magnitude > distance) 
                {
                    // only travel max distance
                    lineR.SetPosition(1, direction.normalized * distance); 
                }
                else
                {
                    // travel to mouse
                    lineR.SetPosition(1, mouse);
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

            lineR.positionCount = 0;
        }
    }
}
