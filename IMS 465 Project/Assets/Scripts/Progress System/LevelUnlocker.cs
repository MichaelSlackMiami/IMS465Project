using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUnlocker : MonoBehaviour
{
    private ProgressTracker ProgressTracker;
    private int[] saveData;
    private int[] starData;

    public int worldIndex;

    public Button[] levels;

    void Update()
    {
        if (ProgressTracker == null)
        {
            ProgressTracker = GameObject.Find("GameManager").GetComponent<ProgressTracker>();

            if (ProgressTracker != null)
            {

                UpdateButtons();
            }
        }
    }

    public void UpdateButtons()
    {
        switch (worldIndex)
        {
            case 1:
                saveData = ProgressTracker.world_1;
                starData = ProgressTracker.stars_1;
                break;
            case 2:
                saveData = ProgressTracker.world_2;
                starData = ProgressTracker.stars_2;
                break;
            case 3:
                saveData = ProgressTracker.world_3;
                starData = ProgressTracker.stars_3;
                break;
            default:
                saveData = ProgressTracker.world_1;
                starData = ProgressTracker.stars_1;
                break;
        }

        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].interactable = saveData[i] == 1;

            switch (starData[i])
            {
                case 1:
                    levels[i].gameObject.transform.Find("Stars/Star_1").gameObject.SetActive(true);
                    levels[i].gameObject.transform.Find("Stars/Star_2").gameObject.SetActive(false);
                    levels[i].gameObject.transform.Find("Stars/Star_3").gameObject.SetActive(false);
                    break;
                case 2:
                    levels[i].gameObject.transform.Find("Stars/Star_1").gameObject.SetActive(true);
                    levels[i].gameObject.transform.Find("Stars/Star_2").gameObject.SetActive(true);
                    levels[i].gameObject.transform.Find("Stars/Star_3").gameObject.SetActive(false);
                    break;
                case 3:
                    levels[i].gameObject.transform.Find("Stars/Star_1").gameObject.SetActive(true);
                    levels[i].gameObject.transform.Find("Stars/Star_2").gameObject.SetActive(true);
                    levels[i].gameObject.transform.Find("Stars/Star_3").gameObject.SetActive(true);
                    break;
                default:
                    levels[i].gameObject.transform.Find("Stars/Star_1").gameObject.SetActive(false);
                    levels[i].gameObject.transform.Find("Stars/Star_2").gameObject.SetActive(false);
                    levels[i].gameObject.transform.Find("Stars/Star_3").gameObject.SetActive(false);
                    break;
            }
        }
    }
}
