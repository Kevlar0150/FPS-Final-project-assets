using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1); //Loads the 1st scene in the build settings which is the main level (NON VR)
    }

    public void StartVRMode()
    {
        Debug.Log("Start VR GAME");
       // SceneManager.LoadScene(2); //Loads the 1st scene in the build settings which is the main level (IN VR)
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
