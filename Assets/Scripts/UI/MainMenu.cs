using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class MainMenu : MonoBehaviour
{
    public float difficultyMultiplier = 1;
    public void Awake()
    {
        DontDestroyOnLoad(this);
    }
    
    public void StartGameNormal()
    {
        SceneManager.LoadScene(1); //Loads the 1st scene in the build settings which is the main level (NON VR)
        difficultyMultiplier = 1;
    }
    public void StartGameHard()
    {
        SceneManager.LoadScene(1); //Loads the 1st scene in the build settings which is the main level (NON VR)
        difficultyMultiplier = 1.5f;
    }
    
    public void StartVRMode()
    {
        Debug.Log("Start VR GAME");

        // Detects whether VR headset is connected
        if (XRDevice.isPresent)
        {
            Debug.Log("YOU CAN PLAY");
            SceneManager.LoadScene(2); //Loads the 1st scene in the build settings which is the main level (IN VR)
        }

        // If VR headset not connected... 
        else
        {
            Debug.Log("Please check if VR device is properly connected and detected by Steam VR");
        }
       
    }
    public float getMultiplier()
    {
        return difficultyMultiplier;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
