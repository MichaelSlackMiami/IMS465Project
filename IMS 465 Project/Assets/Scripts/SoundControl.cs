using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundControl : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    private Slider slider;

    private void Start()
    {
        slider = gameObject.GetComponent<Slider>();

        if (gameObject.name == "MusicSlider")
        {
            slider.value = PlayerPrefs.GetFloat("music_level", 0);
        }
        else if (gameObject.name == "SFXSlider")
        {
            slider.value = PlayerPrefs.GetFloat("sfx_level", 0);
        }
    }

    public void SetVolume()
    {
        float level = slider.value;

        if (gameObject.name == "MusicSlider")
        {

            mixer.SetFloat("MusicVolume", level);
            PlayerPrefs.SetFloat("music_level", level);

        } else if (gameObject.name == "SFXSlider")
        {
            mixer.SetFloat("SfxVolume", level);
            PlayerPrefs.SetFloat("sfx_level", level);
        }
    }
}
