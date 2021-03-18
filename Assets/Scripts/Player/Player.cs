using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sources
// https://docs.unity3d.com/ScriptReference/CharacterController.Move.html - Jump formula
public class Player : MonoBehaviour
{
    // Player properties
    public float health = 100.0f;
    public float maxHealth = 100f;

    // Player Movement
    public CharacterController controller;
    public float defaultSpeed = 10;
    public float speed;

    Vector3 velocity;
    public float gravity = -18.81f;
    public float jumpForce = 2.8f;
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    // Player Buff properties
    public float speedBoostDuration = 5f;
    public float speedBoostTimer;

    //Boo;s
    bool isGrounded;
    bool speedBuffOn = false;

    private void Start()
    {
        speedBoostTimer = speedBoostDuration;
        speed = defaultSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        // ---------------------- Player Movement -----------------------
   
            // * Moving the player *

        // Gets values from Input Axis of Horizontal and Vertical (Default keys are WASD or Arrow Keys)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 movement = transform.right * x + transform.forward * z;
        controller.Move(movement * speed * Time.deltaTime); // moves the GameObject X and Y value in the given direction multiplied by speed value

            // * Player Jump *

        // Create a small physics sphere and checks collision with sphere and any Layers set to groundMask Layer and returns true or false
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) { velocity.y = 0f; } // Sets character y position to 0 so character doesn't fall through ground

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) // If Space bar is pressed and character IS grounded
        {
            //Debug.Log("Jump");
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity); // Increase the players y velocity by Square root of jump height *-2 * gravity. ( Formula Taken from Unity Documentation )
        }
        velocity.y += gravity * Time.deltaTime; // Allows character position y to be manipulated by gravity
        controller.Move(velocity * Time.deltaTime); // // moves the GameObject in all XYZ values in the given direction 

        // -------------------- Player Death ---------------------

        if (health <= 0) // Player dies when health reaches 0
        {
            Destroy(gameObject);
        }

        // -------------------- Power ups ----------------------

        //Speed boost buff
        if (speedBuffOn)
        {
            speedBoostTimer -= Time.deltaTime;
            if (speedBoostTimer <= 0)
            {
                Debug.Log("SPEED BOOST OFF");
                increaseSpeed(1, false);
                speedBoostTimer = speedBoostDuration;
            }
        }
        // Shield buff insert here
    }
    public void TakeDamage(float damage) { health -= damage; }

    // ---------------Power up functions-------------------
    public void increaseHealth(float healthAmount)
    {
        health += healthAmount;

        if (health >= maxHealth) 
        {
            health = maxHealth; // Sets maximum health to be 100 
        }
    }
    public void increaseSpeed(float multiplier, bool speedBoost)
    {
        speedBuffOn = speedBoost;
        if (speedBuffOn) // If true, multiply speed
        {  speed *= multiplier; }

        else // If false, set speed to default.
        {  speed = defaultSpeed; }
    }
}
