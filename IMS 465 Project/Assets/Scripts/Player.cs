using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameManager GameManager;
    [SerializeField] private Grapple myGrapple;

    public bool hasShield = false;
    public bool invincible = false;
    public PhysicsMaterial2D bumperMat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Powerups

    public void ApplyPowerup(int id)
    {
        switch (id)
        {
            case 1:
                Shield();
                break;
            case 2:
                StartCoroutine(Phase());
                break;
            default:
                break;
        }
    }

    public void Shield()
    {
        hasShield = true;
        GetComponent<Rigidbody2D>().sharedMaterial = bumperMat;
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void RemoveShield()
    {
        hasShield = false;
        GetComponent<Rigidbody2D>().sharedMaterial = null;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    IEnumerator Phase()
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(3);
        GetComponent<Collider2D>().enabled = true;
    }

    #endregion

    #region Collisions
    private void OnCollisionStay2D(Collision2D collision)
    {
        // If the collided object is lighter than the player,
        // pass this info to the grapple script for throwing
        if (collision.rigidbody.mass < GetComponent<Rigidbody2D>().mass)
            myGrapple.CheckThrowable(collision.gameObject);
    }

    #endregion
}
