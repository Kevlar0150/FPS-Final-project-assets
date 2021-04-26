using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    // mouse Sensitivity
    public float mouseSens = 500f;

    // Player transform variable
    public Transform playerBody;

    float xRotation = 0f;

    PauseMenu pause;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        pause = FindObjectOfType<PauseMenu>();
        if (pause.GetHasPaused())
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        float mouseXAxis = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime; // Gets mouse position on X axis
        float mouseYAxis = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime; // Gets mouse position on Y axis

        xRotation -= mouseYAxis; 
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamps rotation so can't look behind the character.

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); //Apply rotation.
        playerBody.Rotate(Vector3.up * mouseXAxis); //Allows player to rotate on X axis.
    }


}
