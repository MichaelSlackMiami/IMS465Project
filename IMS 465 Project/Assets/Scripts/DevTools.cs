using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTools : MonoBehaviour
{
    public void UnlockAll()
    {
        ProgressTracker ProgressTracker = GameObject.Find("GameManager").GetComponent<ProgressTracker>();
        if (ProgressTracker)
        {
            ProgressTracker.UnlockAll();
        }
    }

    public void ResetProgress()
    {
        ProgressTracker ProgressTracker = GameObject.Find("GameManager").GetComponent<ProgressTracker>();
        if (ProgressTracker)
        {
            ProgressTracker.ResetProgress();
        }
    }
}
