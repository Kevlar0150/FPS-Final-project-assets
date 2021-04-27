using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Sigtrap.VrTunnellingPro;

public class PauseMenuVR : MonoBehaviour
{

    public  bool hasPaused = false;
    public static bool hasControlInstructionPressed = false;
    public GameObject pauseMenuUI;
    public GameObject ControlInstructionPanel;
    public GameObject HUD;
    private Canvas UICanvas;

    InputDevice deviceR;
    public XRNode rightController;
    public InputDeviceCharacteristics controllerCharacteristics;

    public Camera playerCamera;

    bool motionSicknessOn = false;
    private void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        deviceR = InputDevices.GetDeviceAtXRNode(rightController);
    }
    // Update is called once per frame
    void Update()
    {
        playerCamera = FindObjectOfType<Camera>();
        UICanvas = GetComponent<Canvas>();
        UICanvas.worldCamera = FindObjectOfType<Camera>();
        UICanvas.planeDistance = 1f;

        deviceR.TryGetFeatureValue(CommonUsages.primaryButton, out bool menuClicked);

        if (menuClicked)
        {
            Debug.Log("Menu clicked");
            if (hasPaused)
            {
                
                ResumeGame();
            }
            else
            {
                Debug.Log("HERRO");
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
        Debug.Log("REUMSE");
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
        HUD.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        hasPaused = true;
    }

    public void ToggleMotionSicknessMode()
    {
        motionSicknessOn = !motionSicknessOn;

        if (motionSicknessOn)
        {
            playerCamera.GetComponent<Tunnelling>().enabled = true;
        }
        else
        {
            playerCamera.GetComponent<Tunnelling>().enabled = false;
        }
    }
}
