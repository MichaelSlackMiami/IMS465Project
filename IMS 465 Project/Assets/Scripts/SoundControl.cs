using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundControl : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    public void SetVolume()
    {
        if (gameObject.name == "MusicSlider")
        {
            mixer.SetFloat("MusicVolume", gameObject.GetComponent<Slider>().value);
        } else if (gameObject.name == "SFXSlider")
        {
            mixer.SetFloat("SfxVolume", gameObject.GetComponent<Slider>().value);
        }
    }
}
