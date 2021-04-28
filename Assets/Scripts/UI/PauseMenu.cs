using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Majority of the code such as ResumeGame(), PauseGame(), Quit() was produced following the tutorial by Brackeys (2017) https://www.youtube.com/watch?v=JivuXdrIHK0&t=97s
// The rest of the code was produced 100% by me.

public class PauseMenu : MonoBehaviour
{

    public static bool hasPaused = false;
    public static bool hasControlInstructionPressed = false;
    public GameObject pauseMenuUI;
    public GameObject ControlInstructionPanel;
    public GameObject HUD;
    public MainMenu mainMenuManager;

    private Canvas UICanvas;
    // Update is called once per frame
    void Update()
    {
        mainMenuManager = FindObjectOfType<MainMenu>();
        UICanvas = GetComponent<Canvas>();
        UICanvas.worldCamera = FindObjectOfType<Camera>();
        UICanvas.planeDistance = 2;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (hasPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }

            if (ControlInstructionPanel)
            {
                Back();
            }
        }
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        HUD.SetActive(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        hasPaused = false;
    }

    public void ControlsAndInstructions()
    {
        pauseMenuUI.SetActive(false);
        ControlInstructionPanel.SetActive(true);
    }

    public void Back()
    {
        ControlInstructionPanel.SetActive(false);
        PauseGame();
    }

    public void Quit()
    {
        Time.timeScale = 1;
        DestroyObject(mainMenuManager.gameObject);
        mainMenuManager.QuitToMainMenu();
        
    }

    private void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None; 
        HUD.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        hasPaused = true;
    }

    public bool GetHasPaused()
    {
        return hasPaused;
    }
}
