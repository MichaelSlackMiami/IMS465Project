using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSounds : MonoBehaviour
{
    [SerializeField] private AudioSource mySFX;
    private float mixerVolume;

    // Start is called before the first frame update
    void Start()
    {
        mySFX = gameObject.GetComponent<AudioSource>();
        mySFX.outputAudioMixerGroup.audioMixer.GetFloat("SfxVolume", out mixerVolume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        mySFX.volume = collision.relativeVelocity.magnitude * mixerVolume;
        mySFX.Play();
    }
}
