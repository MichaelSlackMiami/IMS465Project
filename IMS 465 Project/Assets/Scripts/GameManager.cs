using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool paused = false;
    [SerializeField] private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
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
                paused = false;
                Time.timeScale = 1;
            } else
            {
                paused = true;
                Time.timeScale = 0;
            }
        } 
    }
    public void LevelClear()
    {
        // Go to the next level
        Debug.Log("Level clear!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void GameOver(string source)
    {
        // Determine how to end the game based on the source of the Game Over
        // Valid source list: OutOfBounds, Impact, BlackHole
        if (source == "OutOfBounds")
        {
            Destroy(player);
        } else if (source == "Impact")
        {
            Destroy(player);
        } else if (source == "BlackHole")
        {
            Destroy(player);
        } else
        {
            Destroy(player);
        }
    }
}
