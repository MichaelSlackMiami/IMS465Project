using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float pullStrength;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Rigidbody2D>())
        {
            // Direction is towards the center of the gravity
            Vector2 direction = (transform.position - collision.transform.position).normalized;

            // Calculate the distance between the object and the black hole's center
            float distance = (transform.position - collision.transform.position).magnitude;

            // Apply an inverse-square law for the force magnitude
            float magnitude = pullStrength / Mathf.Pow(distance, 2);

            collision.GetComponent<Rigidbody2D>().AddForce(direction * magnitude);
        }
    }
}
