using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryBoard : MonoBehaviour
{
    public GameObject[] slides;

    public GameObject finalSlide;

    void Start()
    {
        StartCoroutine(SlideShow());
    }

    IEnumerator SlideShow()
    {
        foreach (GameObject slide in slides)
        {
            slide.SetActive(true);

            yield return new WaitForSeconds(2);

            slide.SetActive(false);
        }

        finalSlide.SetActive(true);
    }
}
