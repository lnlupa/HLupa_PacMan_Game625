using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DisplayData : MonoBehaviour
{
    private int PacScore = 0;
    private int PacLives = 3;
    public int NumberofDots = 160;

    public Text ScoreText;
    public Text LivesText;

    // Start is called before the first frame update
    void Start()
    {
        PlayerScript.EatDot += CountDots;
        GhostScript.PacKilled += CountLives;
        ScoreText.text = "Points: 0";
        LivesText.text = "Lives: 3";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CountDots() 
    {
        PacScore += 1;
        ScoreText.text = "Points: " + PacScore;
        if (PacScore >= NumberofDots) 
        {
            SceneManager.LoadScene("WinScene", LoadSceneMode.Single);
        }
    }

    private void CountLives() 
    {
        PacLives -= 1;
        LivesText.text = "Lives: " + PacLives;
        if (PacLives <= 0)
        {
            SceneManager.LoadScene("LoseScene", LoadSceneMode.Single);
        }
    }
}
