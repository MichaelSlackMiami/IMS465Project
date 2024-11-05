using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    // Start is called before the first frame update
    void Awake()
    {
        music = GameObject.Find("GameManager").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVolume()
    {
        music.volume = gameObject.GetComponent<Slider>().value;
    }
}
