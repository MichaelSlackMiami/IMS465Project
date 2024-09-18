using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vendelyGrappleTest : MonoBehaviour
{
    [Header("Grapple Properties")]
    public float distance;

    [Header("Hook")]
    public GameObject hook;
    public float hookSize;

    private GameObject myHook;

    [Header("Line")]
    public LineRenderer lineR;

    private Vector2 direction;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("clik");
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = mouse - transform.position;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);

            Debug.Log(hit.collider);


            lineR.positionCount = 2;
            lineR.SetPosition(0, transform.position);

            if (hit.collider)
            {
                myHook = Instantiate(hook, hit.transform, false);
                myHook.transform.position = hit.point;
                myHook.transform.localScale *= hookSize;
                myHook.transform.localRotation = Quaternion.Euler(0, 0, Vector2.Angle(Vector2.up, hit.normal));

                lineR.SetPosition(1, hit.point);
            }
            else
            {

                lineR.SetPosition(1, mouse);
            }


        }


        if (Input.GetMouseButtonUp(0))
        {
            if (myHook)
            {
                Destroy(myHook);
            }

            lineR.positionCount = 0;
        }
    }
}
