using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSounds : MonoBehaviour
{
    [SerializeField] private AudioSource mySFX;

    // Start is called before the first frame update
    void Start()
    {
        mySFX = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        mySFX.volume = PlayerPrefs.GetFloat("sfx_level", 1) * (collision.relativeVelocity.magnitude * 0.5f);
        mySFX.Play();
    }
}
