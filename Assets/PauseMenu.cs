using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public static bool hasPaused = false;
    public static bool hasControlInstructionPressed = false;
    public GameObject pauseMenuUI;
    public GameObject ControlInstructionPanel;
    public GameObject HUD;

    // Update is called once per frame
    void Update()
    {
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

    public void QuitGame()
    {
        Application.Quit();
    }

    private void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None; // Removes cursor from the game.
        HUD.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        hasPaused = true;
    }
}
