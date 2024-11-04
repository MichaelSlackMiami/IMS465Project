using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUnlocker : MonoBehaviour
{
    private ProgressTracker ProgressTracker;
    private int[] saveData;

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
                break;
            case 2:
                saveData = ProgressTracker.world_2;
                break;
            case 3:
                saveData = ProgressTracker.world_3;
                break;
            default:
                saveData = ProgressTracker.world_1;
                break;
        }

        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].interactable = saveData[i] == 1;
        }
    }
}
