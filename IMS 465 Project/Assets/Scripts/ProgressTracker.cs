using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressTracker : MonoBehaviour
{
    public int[] world_1;
    public int[] world_2;
    public int[] world_3;

    private void Start()
    {
        GetSave();
    }

    public void GetSave()
    {
        for (int i = 0; i < world_1.Length; i++)
        {
            world_1[i] = PlayerPrefs.GetInt("world_1_" + i);
        }

        for (int i = 0; i < world_2.Length; i++)
        {
            world_2[i] = PlayerPrefs.GetInt("world_2_" + i);
        }

        for (int i = 0; i < world_3.Length; i++)
        {
            world_3[i] = PlayerPrefs.GetInt("world_3_" + i);
        }

        world_1[0] = 1;
        world_2[0] = 1;
        world_3[0] = 1;
    }

    public void Save()
    {
        for (int i = 0; i < world_1.Length; i++)
        {
            PlayerPrefs.SetInt("world_1_" + i, world_1[i]);
        }

        for (int i = 0; i < world_2.Length; i++)
        {
            PlayerPrefs.SetInt("world_2_" + i, world_2[i]);
        }

        for (int i = 0; i < world_3.Length; i++)
        {
            PlayerPrefs.SetInt("world_3_" + i, world_3[i]);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("Saving...");
        Save();
    }
}
