using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSounds : MonoBehaviour
{
    [SerializeField] private AudioSource mySFX;

    private float baseVolume;
    private float volumeScale = 0.12f;

    // Start is called before the first frame update
    void Start()
    {
        mySFX = gameObject.GetComponent<AudioSource>();
        baseVolume = mySFX.volume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        mySFX.volume = baseVolume * (collision.relativeVelocity.magnitude * volumeScale);
        mySFX.Play();
    }
}
