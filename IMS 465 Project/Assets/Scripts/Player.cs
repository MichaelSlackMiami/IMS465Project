using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameManager GameManager;

    public bool hasShield = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Powerups

    public void ApplyPowerup(int id, float time)
    {
        switch (id)
        {
            case 1:
                Shield();
                break;
            case 2:
                StartCoroutine(Phase(time));
                break;
            default:
                break;
        }
    }

    public void Shield()
    {
        hasShield = true;
        GetComponent<Rigidbody2D>().sharedMaterial.bounciness *= 4;
    }

    public void RemoveShield()
    {
        hasShield = false;
        GetComponent<Rigidbody2D>().sharedMaterial.bounciness /= 4;
    }

    IEnumerator Phase(float time)
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(time);
        GetComponent<Collider2D>().enabled = true;
    }

    #endregion
}
