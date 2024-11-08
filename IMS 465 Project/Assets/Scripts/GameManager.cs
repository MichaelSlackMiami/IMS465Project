using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private bool paused = false;

    [SerializeField] private GameObject player;
    [SerializeField] private Grapple grapple;
    [SerializeField] private Cam cam;

    [Header("Level Clear")]
    [SerializeField] private GameObject LevelClearDisplay1;
    [SerializeField] private GameObject LevelClearDisplay2;

    [Header("Game Over")]
    [SerializeField] private GameObject GameOverDisplay;

    [Header("Pause")]
    [SerializeField] private GameObject PauseDisplay;

    [Header("Music")]
    [SerializeField] private AudioClip[] tracks;
    [SerializeField] private AudioSource primaryMusic;
    [SerializeField] private AudioSource secondaryMusic;
    [SerializeField] private AudioSource nonLoopingMusic;

    private static GameManager instance;

    void Awake()
    {
        // If a GameManager does not already exist...
        if (instance == null)
        {
            // ... Perform GameManager functions
            instance = this;
            DontDestroyOnLoad(gameObject);

            // ... Start music
            primaryMusic.clip = tracks[4];
            primaryMusic.Play();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level != 0 && level != 1 && level != 2 && level != 3)
        {
            // Identify necessary elements in scene
            player = GameObject.Find("Player");
            grapple = GameObject.Find("Grapple").GetComponent<Grapple>();
            cam = GameObject.Find("Main Camera").GetComponent<Cam>();
            LevelClearDisplay1 = GameObject.Find("LevelClearDisplay1");
            LevelClearDisplay1.SetActive(false);
            LevelClearDisplay2 = GameObject.Find("LevelClearDisplay2");
            LevelClearDisplay2.SetActive(false);
            GameOverDisplay = GameObject.Find("GameOverDisplay");
            GameOverDisplay.SetActive(false);
            PauseDisplay = GameObject.Find("PauseDisplay");
            PauseDisplay.SetActive(false);
        }

        if (level == 1)
        {
            // Intro scene
            primaryMusic.Stop();
            // "Mishap"
            nonLoopingMusic.clip = tracks[8];
            nonLoopingMusic.Play();
        } else if (level == 2)
        {
            // World select
            if (primaryMusic.clip != tracks[0])
            {
                primaryMusic.Stop();
                nonLoopingMusic.Stop();
                // "A Good Day to be Stuck in Space"
                primaryMusic.clip = tracks[0];
                primaryMusic.Play();
            }
        } else if (level == 4 || level == 5 || level == 6)
        {
            // In a World 1 level
            if (primaryMusic.clip != tracks[1])
            {
                primaryMusic.Stop();
                // "An Unstoppable Force"
                primaryMusic.clip = tracks[1];
                primaryMusic.Play();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If the player presses P...
        if (Input.GetKeyDown(KeyCode.P))
        {
            // ... If the game is paused, unpause. Otherwise, pause.
            if (paused)
            {
                PauseDisplay.SetActive(false);
                paused = false;
                secondaryMusic.Stop();
                primaryMusic.UnPause();
                Time.timeScale = 1;
            } else
            {
                PauseDisplay.SetActive(true);
                paused = true;
                primaryMusic.Pause();
                // "Everything Stop Exploding for a Minute"
                secondaryMusic.clip = tracks[2];
                secondaryMusic.Play();
                Time.timeScale = 0;
            }
        }

        // If the level has been cleared...
        if (LevelClearDisplay1)
            if (LevelClearDisplay2.activeInHierarchy)
            {
                // ... If the player left clicks...
                if (Input.GetMouseButtonDown(0))
                {
                    // ... Load the World Select menu
                    SceneManager.LoadScene("WorldSelect");
                }
            }

        // If the player lost...
        if (GameOverDisplay)
            if (GameOverDisplay.activeInHierarchy)
            {
                // ... If the player left clicks...
                if (Input.GetMouseButtonDown(0))
                {
                    // ... Reload the current level
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
    }
    public void LevelClear()
    {
        // Lock the camera position and freeze the player
        cam.followPlayer = false;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        grapple.grappleDisabled = true;
        gameObject.GetComponent<ProgressTracker>().LevelClear();
        primaryMusic.Stop();
        nonLoopingMusic.clip = tracks[6];
        nonLoopingMusic.Play();
        StartCoroutine(DisplayText("LevelClear"));
    }

    private IEnumerator DisplayText(string content)
    {
        // Display text based on content
        // Valid content values: "LevelClear", "GameOver"
        if (content == "LevelClear")
        {
            // Display winning text
            LevelClearDisplay1.SetActive(true);
            yield return new WaitForSeconds(3);
            LevelClearDisplay2.SetActive(true);
        } else if (content == "GameOver")
        {
            GameOverDisplay.SetActive(true);
        } else if (content == "Pause")
        {

        }

    }
    public void GameOver(string source)
    {
        // Determine how to end the game based on the source of the Game Over
        // Valid source list: OutOfBounds, Impact, BlackHole, FuelLost
        if (source == "OutOfBounds")
        {

        } else if (source == "Impact")
        {
            Destroy(player);
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        } else if (source == "BlackHole")
        {
            Destroy(player);
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        } else if (source == "FuelLost")
        {
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        } else
        {
            Destroy(player);
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
        // Regardless of how the player lost, do the following
        cam.followPlayer = false;
        StartCoroutine(DisplayText("GameOver"));
    }
}
