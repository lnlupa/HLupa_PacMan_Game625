using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
   public void StartGame() 
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public void QuitGame() 
    {
        Application.Quit();
    }
}
