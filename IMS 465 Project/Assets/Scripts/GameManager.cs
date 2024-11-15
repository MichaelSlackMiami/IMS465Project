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
    [SerializeField] private UI UI;

    public bool gameOver = false;
    public bool levelClear = false;

    [Header("Music")]
    [SerializeField] private AudioClip[] tracks;
    [SerializeField] private AudioSource primaryMusic;
    [SerializeField] private AudioSource secondaryMusic;
    [SerializeField] private AudioSource nonLoopingMusic;

    private static GameManager instance;

    private void Start()
    {
        if (GameObject.Find("Player") != null)
        {
            // Identify necessary elements in scene
            player = GameObject.Find("Player");
            grapple = GameObject.Find("Grapple").GetComponent<Grapple>();
            cam = GameObject.Find("Main Camera").GetComponent<Cam>();
            UI = GameObject.Find("Level UI").GetComponent<UI>();
        }
    
    }

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
            UI = GameObject.Find("Level UI").GetComponent<UI>();
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
            } else if (!primaryMusic.isPlaying)
            {
                primaryMusic.Play();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // PAUSING
        // If the player presses P...
        if (Input.GetKeyDown(KeyCode.P))
        {
            // ... If the game is paused, unpause. Otherwise, pause.
            if (paused)
            {
                paused = false;
                UI.TogglePause(false);
                secondaryMusic.Stop();
                primaryMusic.UnPause();
                Time.timeScale = 1;
                grapple.grappleDisabled = false;
            } else
            {
                paused = true;
                grapple.grappleDisabled = true;
                UI.TogglePause(true);
                primaryMusic.Pause();
                // "Everything Stop Exploding for a Minute"
                secondaryMusic.clip = tracks[2];
                secondaryMusic.Play();
                Time.timeScale = 0;
            }
        }

        // END OF LEVEL
        if (gameOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                gameOver = false;
            }
        } else if (levelClear)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("WorldSelect");
                levelClear = false;
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
        // "Fuel Can't"
        nonLoopingMusic.clip = tracks[6];
        nonLoopingMusic.Play();
        StartCoroutine(UI.DisplayLevelClear());
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
        primaryMusic.Stop();
        UI.DisplayGameOver(source);
    }
}
