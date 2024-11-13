using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    [SerializeField] private GameManager GameManager;
    [SerializeField] private GameObject OutOfBounds;

    private GameObject myOOB;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (myOOB == null)
        {
            // Create Out of Bounds zone
            myOOB = Instantiate(OutOfBounds);
            myOOB.transform.position = transform.position;
            myOOB.transform.localScale = (transform.lossyScale * 2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.GameOver("OutOfBounds");
        } else if (collision.CompareTag("Fuel"))
        {
            GameManager.GameOver("FuelLost");
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (myOOB == null)
        {
            // Create Out of Bounds zone
            myOOB = Instantiate(OutOfBounds);
            myOOB.transform.position = transform.position;
            myOOB.transform.localScale = (transform.lossyScale * 2);
        }
    }
}
