using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slackGrappleTest : MonoBehaviour
{
    //Declare Variables
    [SerializeField] private Transform player;
    [SerializeField] private LineRenderer lineRender;

    // Start is called before the first frame update
    void Start()
    {
        //Set grapple width
        lineRender.startWidth = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D grapple = Physics2D.Raycast(player.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position);
            //Set origin of grapple to player's position
            lineRender.SetPosition(0, player.transform.position);
            if (grapple.collider != null)
            {
                //Grapple hit something
                lineRender.SetPosition(1, grapple.point);
            } else
            {
                //Grapple did not hit anything
                lineRender.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position);
            }
        }
    }
}
