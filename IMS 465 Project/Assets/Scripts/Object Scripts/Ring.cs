using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    private GameObject fuel;
    private Vector3 fuelPosition;
    private Vector3 myPosition;

    // Start is called before the first frame update
    void Start()
    {
        fuel = GameObject.Find("Fuel");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targ = fuel.transform.position;
        targ.z = 0f;

        Vector3 objectPos = transform.position;
        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;

        float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }
}
