using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private GameObject LinkedPortal;

    [SerializeField] private bool reacivates = true;
    [SerializeField] private float cooldownDuration = 3.0f;
    [SerializeField] private bool cooldownActive = false;
    [SerializeField] private bool playerOnly = false;

    [SerializeField] private bool enterNotStay = false;

    [SerializeField] private AudioSource mySFX;

    // Start is called before the first frame update
    void Start()
    {
        mySFX = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 0.05f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!enterNotStay)
        {
            // If this portal is not on cooldown...
            if (cooldownActive == false && (!playerOnly || (playerOnly && collision.CompareTag("Player"))))
            {
                // ... Teleport the object that collided with the portal to the linked portal
                collision.transform.position = LinkedPortal.transform.position;
                gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
                LinkedPortal.GetComponent<SpriteRenderer>().color = Color.gray;

                // ... Play SFX
                mySFX.Play();

                // ... Cancel the current grapple
                GameObject.Find("Grapple").GetComponent<Grapple>().RetractArm();

                // If this portal should reactivate...
                if (reacivates)
                {
                    // ... Begin cooldown
                    StartCoroutine(CooldownTimer());
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enterNotStay)
        {
            // If this portal is not on cooldown...
            if (cooldownActive == false && (!playerOnly || (playerOnly && collision.CompareTag("Player"))))
            {
                // ... Teleport the object that collided with the portal to the linked portal
                collision.transform.position = LinkedPortal.transform.position;
                gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
                LinkedPortal.GetComponent<SpriteRenderer>().color = Color.gray;

                // ... Play SFX
                mySFX.Play();

                // ... Cancel the current grapple
                GameObject.Find("Grapple").GetComponent<Grapple>().RetractArm();

                // If this portal should reactivate...
                if (reacivates)
                {
                    // ... Begin cooldown
                    StartCoroutine(CooldownTimer());
                }
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
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        LinkedPortal.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
