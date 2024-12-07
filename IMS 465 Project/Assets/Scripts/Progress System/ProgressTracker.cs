using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressTracker : MonoBehaviour
{
    public int[] world_1;
    public int[] world_2;
    public int[] world_3;

    public int[] stars_1;
    public int[] stars_2;
    public int[] stars_3;

    public bool story_w1, story_w2, story_w3, story_intro, story_outro;

    private void Start()
    {
        GetSave();
    }

    public void GetSave()
    {
        // Unlocked Levels
        for (int i = 0; i < world_1.Length; i++)
            world_1[i] = PlayerPrefs.GetInt("world_1_" + i);

        for (int i = 0; i < world_2.Length; i++)
            world_2[i] = PlayerPrefs.GetInt("world_2_" + i);

        for (int i = 0; i < world_3.Length; i++)
            world_3[i] = PlayerPrefs.GetInt("world_3_" + i);

        // Stars
        for (int i = 0; i < stars_1.Length; i++)
            stars_1[i] = PlayerPrefs.GetInt("stars_1_" + i);

        for (int i = 0; i < stars_2.Length; i++)
            stars_2[i] = PlayerPrefs.GetInt("stars_2_" + i);

        for (int i = 0; i < stars_3.Length; i++)
            stars_3[i] = PlayerPrefs.GetInt("stars_3_" + i);

        world_1[0] = 1;
        world_2[0] = 1;
        world_3[0] = 1;

        story_w1 = PlayerPrefs.GetInt("story_w1") == 1;
        story_w2 = PlayerPrefs.GetInt("story_w2") == 1;
        story_w3 = PlayerPrefs.GetInt("story_w3") == 1;
        story_intro = PlayerPrefs.GetInt("story_intro") == 1;
        story_outro = PlayerPrefs.GetInt("story_outro") == 1;
    }

    public void Save()
    {
        for (int i = 0; i < world_1.Length; i++)
            PlayerPrefs.SetInt("world_1_" + i, world_1[i]);

        for (int i = 0; i < world_2.Length; i++)
            PlayerPrefs.SetInt("world_2_" + i, world_2[i]);

        for (int i = 0; i < world_3.Length; i++)
            PlayerPrefs.SetInt("world_3_" + i, world_3[i]);

        for (int i = 0; i < stars_1.Length; i++)
            PlayerPrefs.SetInt("stars_1_" + i, stars_1[i]);

        for (int i = 0; i < stars_2.Length; i++)
            PlayerPrefs.SetInt("stars_2_" + i, stars_2[i]);

        for (int i = 0; i < stars_3.Length; i++)
            PlayerPrefs.SetInt("stars_3_" + i, stars_3[i]);

        PlayerPrefs.SetInt("story_w1", story_w1 ? 1 : 0);
        PlayerPrefs.SetInt("story_w2", story_w2 ? 1 : 0);
        PlayerPrefs.SetInt("story_w3", story_w3 ? 1 : 0);
        PlayerPrefs.SetInt("story_intro", story_intro ? 1 : 0);
        PlayerPrefs.SetInt("story_outro", story_outro ? 1 : 0);

        PlayerPrefs.Save();

        Debug.Log("Saved");
    }

    public void LevelClear()
    {
        LevelData ld = GameObject.Find("LevelData").GetComponent<LevelData>();

        // Stop timer
        ld.counting = false;

        // Get level number
        int lvl = ld.levelIndex;

        // Default arrays
        int[] world = world_1;
        int[] stars = stars_1;

        // Get the correct arrays
        switch (ld.worldIndex)
        {
            case 2:
                world = world_2;
                stars = stars_2;
                break;
            case 3:
                world = world_3;
                stars = stars_3;
                break;
            default:
                break;
        }

        // Unlock next level
        if (lvl + 1 < world.Length)
            world[lvl + 1] = 1;
                // else if (ld.levelIndex + 1 == world_1.Length)
                //     play the chapter ending storyboard

        // Update stars for finished level
        if (stars[lvl] < 3 && ld.time < ld.star3time)
            stars[lvl] = 3;

        else if (stars[lvl] < 2 && ld.time < ld.star2time)
            stars[lvl] = 2;

        else if (stars[lvl] < 1)
            stars[lvl] = 1;
       
        Save();
    }

    public void ResetProgress()
    {
        for (int i = 0; i < world_1.Length; i++)
            world_1[i] = 0;

        for (int i = 0; i < world_2.Length; i++)
            world_2[i] = 0;

        for (int i = 0; i < world_3.Length; i++)
            world_3[i] = 0;

        for (int i = 0; i < stars_1.Length; i++)
            stars_1[i] = 0;

        for (int i = 0; i < stars_2.Length; i++)
            stars_2[i] = 0;

        for (int i = 0; i < stars_3.Length; i++)
            stars_3[i] = 0;


        world_1[0] = 1;
        world_2[0] = 1;
        world_3[0] = 1;

        story_w1 = false;
        story_w2 = false;
        story_w3 = false;
        story_intro = false;
        story_outro = false;

        Save();
    }

    public void UnlockAll()
    {
        for (int i = 0; i < world_1.Length; i++)
            world_1[i] = 1;

        for (int i = 0; i < world_2.Length; i++)
            world_2[i] = 1;

        for (int i = 0; i < world_3.Length; i++)
            world_3[i] = 1;

        story_w1 = true;
        story_w2 = true;
        story_w3 = true;
        story_intro = true;
        story_outro = true;

        Save();
    }
}
