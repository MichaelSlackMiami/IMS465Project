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

            // ... Start "Resolve"
            primaryMusic.clip = tracks[0];
            primaryMusic.Play();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        // If this is not a menu or story scene...
        if (level != 0 && level != 1 && level != 2 && level != 3 && level != 4 && level != 15 && level != 16 && level != 27 && level != 28 && level != 39)
        {
            // ... Identify necessary elements in scene
            player = GameObject.Find("Player");
            grapple = GameObject.Find("Grapple").GetComponent<Grapple>();
            cam = GameObject.Find("Main Camera").GetComponent<Cam>();
            UI = GameObject.Find("Level UI").GetComponent<UI>();
        }

        if (level == 1)
        {
            // Intro scene
            primaryMusic.Stop();
            // "A Mission"
            nonLoopingMusic.clip = tracks[1];
            nonLoopingMusic.Play();
        }
        else if (level == 3 || level == 15 || level == 27)
        {
            // Story scene
            primaryMusic.Stop();
            // "Mishap"
            nonLoopingMusic.clip = tracks[3];
            nonLoopingMusic.Play();
        }
        else if (level == 2 || level == 4 || level == 16 || level == 28)
        {
            // World select
            nonLoopingMusic.Stop();
            if (primaryMusic.clip != tracks[2] || !primaryMusic.isPlaying)
            {
                primaryMusic.Stop();
                // "A Good Day to be Stuck in Space"
                primaryMusic.clip = tracks[2];
                primaryMusic.Play();
            }
        } else if (level > 4 && level < 15)
        {
            // In a World 1 level
            if (primaryMusic.clip != tracks[4])
            {
                primaryMusic.Stop();
                // "The Journey"
                primaryMusic.clip = tracks[4];
                primaryMusic.Play();
            } else if (!primaryMusic.isPlaying)
            {
                primaryMusic.Play();
            }
        }
        else if (level > 16 && level < 27)
        {
            // In a World 2 level
            if (primaryMusic.clip != tracks[5])
            {
                primaryMusic.Stop();
                // "An Unstoppable Force"
                primaryMusic.clip = tracks[5];
                primaryMusic.Play();
            }
            else if (!primaryMusic.isPlaying)
            {
                primaryMusic.Play();
            }
        }
        else if (level > 28 && level < 39)
        {
            // In a World 3 level
            if (primaryMusic.clip != tracks[6])
            {
                primaryMusic.Stop();
                // "Peril"
                primaryMusic.clip = tracks[6];
                primaryMusic.Play();
            }
            else if (!primaryMusic.isPlaying)
            {
                primaryMusic.Play();
            }
        } else if (level == 39)
        {
            // Closing scene
            primaryMusic.Stop();
            // "The Destination"
            nonLoopingMusic.clip = tracks[7];
            nonLoopingMusic.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
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
    public void TogglePause(bool pause)
    {
        // ... Set the pause state according to input
        if (pause)
        {
            grapple.grappleDisabled = true;
            primaryMusic.Pause();
            // "Everything Stop Exploding for a Minute"
            secondaryMusic.clip = tracks[8];
            secondaryMusic.Play();
            Time.timeScale = 0;
        }
        else
        {
            secondaryMusic.Stop();
            primaryMusic.UnPause();
            Time.timeScale = 1;
            grapple.grappleDisabled = false;
        }
    }
    
    public void LevelClear()
    {
        // Lock the camera position and freeze the player
        cam.followPlayer = false;
        player.GetComponent<Player>().invincible = true;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        grapple.grappleDisabled = true;
        gameObject.GetComponent<ProgressTracker>().LevelClear();
        primaryMusic.Stop();
        if (SceneManager.GetActiveScene().buildIndex == 14 || SceneManager.GetActiveScene().buildIndex == 26 || SceneManager.GetActiveScene().buildIndex == 38)
        {
            // "Fuel CAN!"
            nonLoopingMusic.clip = tracks[11];
            nonLoopingMusic.Play();
            StartCoroutine(UI.DisplayWorldClear());
        } else
        {
            // "Fuel Can't"
            nonLoopingMusic.clip = tracks[10];
            nonLoopingMusic.Play();
            StartCoroutine(UI.DisplayLevelClear());
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
        primaryMusic.Stop();
        UI.DisplayGameOver(source);
    }
}
