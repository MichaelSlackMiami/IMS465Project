using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    [SerializeField] private AudioSource[] music;
    // Start is called before the first frame update
    void Awake()
    {
        music = GameObject.Find("GameManager").GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVolume()
    {
        foreach (AudioSource source in music)
        {
            source.volume = gameObject.GetComponent<Slider>().value;
        }
    }
}
