using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField] private GameManager GM;
    [SerializeField] private LevelData LD;
    [SerializeField] private GameObject TextBG;

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
    [SerializeField] private GameObject txtClickRetry;

    [Header("Pause")]
    [SerializeField] private GameObject PauseBG;
    [SerializeField] private GameObject txtPause;
    [SerializeField] private GameObject btnExitLevel;
    [SerializeField] private Image btnIcon;
    [SerializeField] private Sprite pauseIcon;
    [SerializeField] private Sprite resumeIcon;
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

        // Hide all UI elements
        TextBG.SetActive(false);
        txtFuelFound.SetActive(false);
        txtFuelEmpty.SetActive(false);
        txtClickContinue.SetActive(false);
        txtGameOver.SetActive(false);
        txtClickRetry.SetActive(false);
        PauseBG.SetActive(false);
        txtPause.SetActive(false);
        btnExitLevel.SetActive(false);
        txtWorldClear.SetActive(false);
        txtFuelFull.SetActive(false);
        txtClickNext.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TogglePause()
    {
        paused = !paused;
        GM.TogglePause(paused);
        PauseBG.SetActive(paused);
        txtPause.SetActive(paused);
        btnExitLevel.SetActive(paused);
        if (paused)
        {
            if (btnIcon)
                btnIcon.sprite = resumeIcon;
        } else
        {
            if (btnIcon)
                btnIcon.sprite = pauseIcon;
        }
    }

    public void ExitLevel()
    {
        GM.TogglePause(false);
        TogglePause();

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
        TextBG.SetActive(true);
        txtFuelFound.SetActive(true);
        DisplayStars();
            
        yield return new WaitForSeconds(4.5f);
        txtFuelEmpty.SetActive(true);
        txtClickContinue.SetActive(true);
        GM.levelClear = true;
    }
    public IEnumerator DisplayWorldClear()
    {
        TextBG.SetActive(true);
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
        TextBG.SetActive(true);
        txtGameOver.SetActive(true);
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
        txtClickRetry.SetActive(true);
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
}
