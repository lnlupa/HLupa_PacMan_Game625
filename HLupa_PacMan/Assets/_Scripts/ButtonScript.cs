using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    //simple script for buttons, loading the game and quitting the game
   public void StartGame() 
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public void QuitGame() 
    {
        Application.Quit();
    }
}
