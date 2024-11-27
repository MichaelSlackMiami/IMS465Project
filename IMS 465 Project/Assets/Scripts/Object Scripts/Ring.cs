using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    private GameObject fuel;
    private SpriteRenderer sR;
    private Color color;
    private Vector3 fuelPosition;
    private Vector3 myPosition;

    public float period;
    private float visTime;
    public float visDuration;
    private float blinkTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        fuel = GameObject.Find("Fuel");

        sR = GetComponent<SpriteRenderer>();
        color = sR.color;

        visTime = period * 0.5f;
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

        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        blinkTimer += Time.deltaTime;

        float percent = (visDuration - Mathf.Abs(blinkTimer - visTime)) / visDuration;

        if (percent > 0.0f)
        {
            color.a = percent;

            sR.color = color;
        }
        else
        {
            color.a = 0;

            sR.color = color;
        }


        if (blinkTimer > period)
        {
            blinkTimer = 0;
        }
    }
}
