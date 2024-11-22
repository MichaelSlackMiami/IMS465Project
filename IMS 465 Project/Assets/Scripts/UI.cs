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

    [Header("Game Over")]
    [SerializeField] private GameObject txtGameOver;
    [SerializeField] private GameObject txtClickRetry;

    [Header("Pause")]
    [SerializeField] private GameObject PauseBG;
    [SerializeField] private GameObject txtPause;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TogglePause(bool paused)
    {
        PauseBG.SetActive(paused);
        txtPause.SetActive(paused);
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
