using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore;

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

    private static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnLevelWasLoaded(int level)
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
                Time.timeScale = 1;
            } else
            {
                PauseDisplay.SetActive(true);
                paused = true;
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
        gameObject.GetComponent<AudioSource>().Play();
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
        // Valid source list: OutOfBounds, Impact, BlackHole
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
