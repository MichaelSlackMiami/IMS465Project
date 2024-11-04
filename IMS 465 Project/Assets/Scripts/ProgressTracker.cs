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
        Debug.Log("Saving...");

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

    public void LevelClear()
    {
        LevelData ld = GameObject.Find("LevelData").GetComponent<LevelData>();

        if (ld != null)
            PlayerPrefs.SetInt("world_" + ld.worldIndex + "_" + ld.levelIndex, 1);

        Save();
    }

    public void ResetProgress()
    {
        for (int i = 0; i < world_1.Length; i++)
        {
            world_1[i] = 0;
        }

        for (int i = 0; i < world_2.Length; i++)
        {
            world_2[i] = 0;
        }

        for (int i = 0; i < world_3.Length; i++)
        {
            world_3[i] = 0;
        }

        world_1[0] = 1;
        world_2[0] = 1;
        world_3[0] = 1;
    }

    public void UnlockAll()
    {
        for (int i = 0; i < world_1.Length; i++)
        {
            world_1[i] = 1;
        }

        for (int i = 0; i < world_2.Length; i++)
        {
            world_2[i] = 1;
        }

        for (int i = 0; i < world_3.Length; i++)
        {
            world_3[i] = 1;
        }
    }
}
