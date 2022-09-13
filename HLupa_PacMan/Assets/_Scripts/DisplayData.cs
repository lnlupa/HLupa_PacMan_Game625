using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DisplayData : MonoBehaviour
{
    //variables that hold the info for the UI
    private int PacScore = 0;
    private int PacLives = 3;

    //the number of points needed to win the game
    public int PointThreshold = 160;

    //text that appears onscreen
    public Text ScoreText;
    public Text LivesText;

    public GameObject ScoreObj;
    public GameObject LivesObj;

    public Text LoseText;
    public Text WinText;

    // Start is called before the first frame update
    void Start()
    {
        ScoreObj = GameObject.FindGameObjectWithTag("ScoreText");
        LivesObj = GameObject.FindGameObjectWithTag("LivesText");

        ScoreText = ScoreObj.GetComponent<Text>();
        LivesText = LivesObj.GetComponent<Text>();


        //messages the UI receives from each of the various scripts
        PlayerScript.EatDot += CountDots;
        GhostScript.PacKilled += CountLives;
        GhostScript.GhostKilled += GhostBonus;
        CherryScript.CherryAte += CherryBonus;

        //starting number of points and lives
        ScoreText.text = "Points: 0";
        LivesText.text = "Lives: 3";
    }

    //function that decrements Pac-Man's lives when he is caught by ghosts
    //and ends the round when he runs out of lives
    private void CountLives() 
    {
        PacLives -= 1;
        LivesText.text = "Lives: " + PacLives;
        if (PacLives <= 0)
        {
            SceneManager.LoadScene("LoseScene", LoadSceneMode.Single);
        }
    }

    //three functions that increment the score when Pac-Man eats dots, eats a ghost in Fear Mode, or gets a cherry
    private void CountDots() 
    {
        PacScore += 1;
        ScoreText.text = "Points: " + PacScore;
        if (PacScore >= PointThreshold) 
        {
            SceneManager.LoadScene("WinScene", LoadSceneMode.Single);
        }
    }

    private void GhostBonus() 
    {
        PacScore += 5; 
        ScoreText.text = "Points: " + PacScore;
        if (PacScore >= PointThreshold)
        {
            SceneManager.LoadScene("WinScene", LoadSceneMode.Single);
        }
    }

    private void CherryBonus() 
    {
        PacScore += 5;
        ScoreText.text = "Points: " + PacScore;
        if (PacScore >= PointThreshold)
        {
            SceneManager.LoadScene("WinScene", LoadSceneMode.Single);
        }
    }

    //at the end of the game we unsubscribe just in case
    private void OnApplicationQuit()
    {
        PlayerScript.EatDot -= CountDots;
        GhostScript.PacKilled -= CountLives;
        GhostScript.GhostKilled -= GhostBonus;
        CherryScript.CherryAte -= CherryBonus;
    }
}
