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
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        myOOB = Instantiate(OutOfBounds);
        myOOB.transform.position = transform.position;
        myOOB.transform.localScale = (transform.lossyScale * 2);
    }
}
