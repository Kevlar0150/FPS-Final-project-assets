using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sources
// The Player Movement variables, Walk() function, Jump() function has been produced following tutorial from Brackeys, 2019, https://www.youtube.com/watch?v=_QajrabyTJc
// The rest of the code has been created by me
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
    private float maxSpeedIncrease = 15;

    //Bools
    bool speedBuffOn = false;
    bool hit = false;
    bool isGrounded;

    // Sound manager
    private SoundManager soundManager;

    // Menu
    private MainMenu menu;
    private void Start()
    {
        speedBoostTimer = speedBoostDuration;
        speed = defaultSpeed;
        soundManager = FindObjectOfType<SoundManager>();
        menu = FindObjectOfType<MainMenu>();
    }
    // Update is called once per frame
    void Update()
    {
        // ---------------------- Player Movement -----------------------

        // * Moving the player *

        Walk();

        // * Player Jump *

        Jump();

        controller.Move(velocity * Time.deltaTime); // // moves the GameObject in all XYZ values in the given direction 

        // -------------------- Player Death ---------------------

        if (health <= 0) // Player dies when health reaches 0
        {
            menu.QuitToMainMenu();
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
    }

    private void Walk()
    {
        // Gets values from Input Axis of Horizontal and Vertical (Default keys are WASD or Arrow Keys)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 movement = transform.right * x + transform.forward * z;
        controller.Move(movement * speed * Time.deltaTime); // moves the GameObject X and Y value in the given direction multiplied by speed value
    }

    private void Jump()
    {
        // Create a small physics sphere and checks collision with sphere and any Layers set to groundMask Layer and returns true or false
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) { velocity.y = 0f; } // Sets character y position to 0 so character doesn't fall through ground

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) // If Space bar is pressed and character IS grounded
        {
            //Debug.Log("Jump");
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity); // Increase the players y velocity by Square root of jump height *-2 * gravity. ( Formula Taken from Unity Documentation )
        }
        velocity.y += gravity * Time.deltaTime; // Allows character position y to be manipulated by gravity
    }

    public void TakeDamage(float damage) 
    {
        soundManager.PlaySound("playerhit");
        health -= damage; 
    }

    // ---------------Power up functions-------------------
    public void increaseHealth(float healthAmount)
    {
        Debug.Log("i am health");
        soundManager.PlaySound("healthup");
        health += healthAmount;

        if (health >= maxHealth) 
        {
            health = maxHealth; // Sets maximum health to be 100 
        }
    }
    public void increaseSpeed(float multiplier, bool speedBoost)
    {
        Debug.Log("I AM SPEED");
        soundManager.PlaySound("speedup");
        speedBuffOn = speedBoost;
        if (speedBuffOn) // If true, multiply speed
        {
            speed *= multiplier;
            if (speed >= maxSpeedIncrease)
            {
                speed = maxSpeedIncrease;
            }
        }

        else // If false, set speed to default.
        {  speed = defaultSpeed; }
    }
    public float getSpeed()
    {
        return speed;
    }
    public float getGravity()
    {
        return gravity;      
    }
    public float getJumpForce()
    {
        return jumpForce;
    }

    //Detects any hitboxes entering players collision box
    private void OnTriggerEnter(Collider other)
    {
        if (!hit)
        {
            if (other.gameObject.tag == "BossRightLeg")
            {
                TakeDamage(35);
                hit = true;
                Invoke("ResetPlayerHit", 0.7f);
            }
        }      
    }
    
    private void ResetPlayerHit()
    {
        hit = false;
    }
}
