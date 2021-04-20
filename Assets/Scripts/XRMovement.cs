using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRMovement : MonoBehaviour
{
    public float speed;
    public XRNode leftController;
    public XRNode rightController;

    Vector3 velocity;

    // Jump variables
    public float gravity;
    public float jumpForce;
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;
    private bool isGrounded = true;

    // References
    private XRRig rig;
    private Vector2 inputAxis;
    private CharacterController controller;
    [SerializeField] GameObject player;
    Player playerScript;

    // Start is called before the first frame update
    void Awake()
    {
        // Get components
        rig = GetComponent<XRRig>();
        controller = transform.parent.GetComponent<CharacterController>();
        playerScript = player.GetComponent<Player>();

        // Movement Variables match non VR Player properties
        speed = playerScript.getSpeed();
        gravity = playerScript.getGravity();
        jumpForce = playerScript.getJumpForce();
    }

    // Update is called once per frame
    void Update()
    {
        // Listens to inputs only from left controll
        InputDevice device = InputDevices.GetDeviceAtXRNode(leftController);

        // Gets value from the touchpad
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);

        Move(); // Moves player in direction

        Jump(); // Makes player jump

        controller.Move(velocity * Time.deltaTime); // moves the GameObject in all XYZ values in the given direction every frame. 
    }

    private void Move()
    {
        // Retrieves speed value from Player script
        speed = playerScript.getSpeed();

        // Sets the forward movement to wherever the VR headset is facing
        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);

        // Moves player in the given direction at the given speed
        controller.Move(direction * speed * Time.deltaTime);
    }

    private void Jump()
    {
        // Create a small physics sphere and checks collision with sphere and any Layers set to groundMask Layer and returns true or false
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) { velocity.y = 0f; } // Sets character y position to 0 so character doesn't fall through ground

        InputDevice device = InputDevices.GetDeviceAtXRNode(leftController);
        device.TryGetFeatureValue(CommonUsages.primary2DAxisClick,  out bool axisClickValue);
        if (axisClickValue && isGrounded) // If Space bar is pressed and character IS grounded
        {
            Debug.Log("Jump");
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity); // Increase the players y velocity by Square root of jump height *-2 * gravity. ( Formula Taken from Unity Documentation )
        }
        velocity.y += gravity * Time.deltaTime; // Allows character position y to be manipulated by gravity
    }
}
