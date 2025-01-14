using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField] private GameManager GM;
    [SerializeField] private LevelData LD;

    [Header("Menus")]
    [SerializeField] private GameObject LevelClearMenu;
    [SerializeField] private GameObject WorldClearMenu;
    [SerializeField] private GameObject GameOverMenu;
    [SerializeField] private GameObject PauseMenu;

    [Header("Level Clear")]
    [SerializeField] private GameObject txtFuelFound;
    [SerializeField] private GameObject txtFuelEmpty;
    [SerializeField] private GameObject txtClickContinue;

    [Header("World Clear")]
    [SerializeField] private GameObject txtWorldClear;
    [SerializeField] private GameObject txtFuelFull;
    [SerializeField] private GameObject txtClickNext;

    [Header("Game Over")]
    [SerializeField] private GameObject txtGameOver;

    [Header("Pause")]
    [SerializeField] private GameObject btnPause;
    [SerializeField] private Image btnPauseIcon;
    [SerializeField] private Sprite resumeIcon;
    [SerializeField] private Sprite pauseIcon;
    private bool paused = false;

    [Header("Stars")]
    [SerializeField] public GameObject star1;
    [SerializeField] public GameObject star2;
    [SerializeField] public GameObject star3;

    // Start is called before the first frame update
    void Start()
    {
        // Connect to the GM
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GM == null)
        {
            // Connect to the GM
            GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }

    public void TogglePause()
    {
        paused = !paused;
        PauseMenu.SetActive(paused);
        GM.TogglePause(paused);
        if (paused)
        {
            btnPauseIcon.sprite = resumeIcon;
        } else
        {
            btnPauseIcon.sprite = pauseIcon;
        }
    }

    public void ExitLevel()
    {
        ProgressTracker PT = GM.GetComponent<ProgressTracker>();
        Debug.Log("Got PT!");

        if (!PT.story_outro && PT.stars_1[9] > 0 && PT.stars_2[9] > 0 && PT.stars_3[9] > 0)
        {
            Debug.Log("Win Scene!");
            PT.story_outro = true;
            PT.Save();
            SceneManager.LoadScene("StoryScene_Ending");
            return;
        }

        SceneManager.LoadScene("WorldSelect");
    }

    public IEnumerator DisplayLevelClear()
    {
        LevelClearMenu.SetActive(true);
        btnPause.SetActive(false);
        txtFuelFound.SetActive(true);
        DisplayStars();
            
        yield return new WaitForSeconds(4.5f);
        txtFuelEmpty.SetActive(true);
        txtClickContinue.SetActive(true);
        GM.levelClear = true;
    }
    public IEnumerator DisplayWorldClear()
    {
        WorldClearMenu.SetActive(true);
        btnPause.SetActive(false);
        txtWorldClear.SetActive(true);
        yield return new WaitForSeconds(1.8f);
        txtFuelFull.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        txtClickNext.SetActive(true);
        DisplayStars();
        GM.levelClear = true;
    }

    public void DisplayGameOver(string source)
    {
        GameOverMenu.SetActive(true);
        GameOverMenu.GetComponentInChildren<Toggle>().isOn = GM.showPreview;
        btnPause.SetActive(false);
        if (source == "Impact")
        {
            int message = Random.Range(1, 4);
            if (message == 1)
            {
                txtGameOver.GetComponent<Text>().text = "That's gonna leave a mark...";
            } else if (message == 2)
            {
                txtGameOver.GetComponent<Text>().text = "That thing came outta nowhere!";
            } else if (message == 3)
            {
                txtGameOver.GetComponent<Text>().text = "At least no one was around to see that.";
            }
            
        } else if (source == "OutOfBounds")
        {
            txtGameOver.GetComponent<Text>().text = "Lost in space...";
        } else if (source == "BlackHole")
        {
            txtGameOver.GetComponent<Text>().text = "Maybe stay away from that next time.";
        } else if (source == "Incineration")
        {
            int message = Random.Range(1, 4);
            if (message == 1)
            {
                txtGameOver.GetComponent<Text>().text = "Little toasty in here.";
            }
            else if (message == 2)
            {
                txtGameOver.GetComponent<Text>().text = "So THAT'S how Icarus felt...";
            }
            else if (message == 3)
            {
                txtGameOver.GetComponent<Text>().text = "Crispy.";
            }
        }
        else if (source == "FuelLost")
        {
            txtGameOver.GetComponent<Text>().text = "The fuel seems to have drifted off!";
        }
        GM.gameOver = true;
    }

    public void DisplayStars()
    {
        LD = GameObject.Find("LevelData").GetComponent<LevelData>();

        if (LD)
        {
            star1.SetActive(true);

            if (LD.star2time > LD.time)
                star2.SetActive(true);

            if (LD.star3time > LD.time)
                star3.SetActive(true);
        }
    }

    public void RetryLevel()
    {
        // If the game was paused when Retry was selected...
        if (paused)
        {
            // ... Unpause the game before restarting
            GM.TogglePause(false);
            TogglePause();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ToWorldSelect()
    {
        GM.TogglePause(false);
        TogglePause();
        SceneManager.LoadScene(2);
    }

    public void TogglePreview()
    {
        GM.showPreview = !GM.showPreview;
    }
}
