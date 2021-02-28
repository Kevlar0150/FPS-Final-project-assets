using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    public float mouseSens = 350f;

    public Transform playerBody;

    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Removes cursor from the game.
    }

    // Update is called once per frame
    void Update()
    {
        float mouseXAxis = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime; // Gets mouse position on X axis
        float mouseYAxis = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime; // Gets mouse position on Y axis

        xRotation -= mouseYAxis; 
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamps rotation so can't look behind the character.

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); //Apply rotation.
        playerBody.Rotate(Vector3.up * mouseXAxis); //Allows player to rotate on X axis.
    }
}
