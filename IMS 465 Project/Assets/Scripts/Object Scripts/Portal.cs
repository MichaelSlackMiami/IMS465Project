using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private GameObject LinkedPortal;

    [SerializeField] private bool reacivates = true;
    [SerializeField] private float cooldownDuration = 3.0f;
    [SerializeField] private bool cooldownActive = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If this portal is not on cooldown...
        if (cooldownActive == false)
        {
            // ... Teleport the object that collided with the portal to the linked portal
            collision.transform.position = LinkedPortal.transform.position;

            // If this portal should reactivate...
            if (reacivates)
            {
                // ... Begin cooldown
                StartCoroutine(CooldownTimer());
            } else
            {
                Destroy(gameObject);
                Destroy(LinkedPortal);
            }
        }
    }

    IEnumerator CooldownTimer()
    {
        // Disable portal and linked portal until cooldown is over
        cooldownActive = true;
        LinkedPortal.GetComponent<Portal>().cooldownActive = true;
        yield return new WaitForSeconds(cooldownDuration);
        cooldownActive = false;
        LinkedPortal.GetComponent<Portal>().cooldownActive = false;
    }
}
