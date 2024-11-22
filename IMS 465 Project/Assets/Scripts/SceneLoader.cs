using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneName;

    public int story;
    public bool shownStory;

    private ProgressTracker ProgressTracker;

    private void Update()
    {
        if (ProgressTracker == null)
        {
            ProgressTracker = GameObject.Find("GameManager").GetComponent<ProgressTracker>();
        }
    }

    public void LoadScene()
    {
        if (ProgressTracker)
        {
            switch (story)
            {
                case 1:
                    shownStory = ProgressTracker.story_w1;

                    if (!shownStory)
                    {
                        ProgressTracker.story_w1 = true;
                        ProgressTracker.Save();
                        SceneManager.LoadScene("StoryScene_w1");
                    }
                    else
                    {
                        SceneManager.LoadScene(sceneName);
                    }
                    break;
                case 2:
                    shownStory = ProgressTracker.story_w2;

                    if (!shownStory)
                    {
                        ProgressTracker.story_w2 = true;
                        ProgressTracker.Save();
                        SceneManager.LoadScene("StoryScene_w2");
                    }
                    else
                    {
                        SceneManager.LoadScene(sceneName);
                    }
                    break;
                case 3:
                    shownStory = ProgressTracker.story_w3;

                    if (!shownStory)
                    {
                        ProgressTracker.story_w3 = true;
                        ProgressTracker.Save();
                        SceneManager.LoadScene("StoryScene_w3");
                    }
                    else
                    {
                        SceneManager.LoadScene(sceneName);
                    }
                    break;
                default:
                    SceneManager.LoadScene(sceneName);
                    break;
            }
        }
    }
}
