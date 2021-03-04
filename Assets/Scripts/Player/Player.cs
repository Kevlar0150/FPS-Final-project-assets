using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Player properties
    public float health = 100.0f;
    public float maxHealth = 100f;

    // Player Movement
    public CharacterController controller;
    public float speed = 13.8f;

    Vector3 velocity;
    public float gravity = -18.81f;
    public float jumpForce = 2.8f;
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        // Create a small physics sphere and checks collision with sphere and any Layers set to groundMask Layer and returns true or false
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); 

        if (isGrounded && velocity.y < 0) // Sets character y position to 0 so character doesn't fall through ground
        {
            //Debug.Log("Grounded");
            velocity.y = 0f;
        }

        // Gets values from Input Axis of Horizontal and Vertical (Default keys are WASD or Arrow Keys)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * x + transform.forward * z;

        controller.Move(movement * speed * Time.deltaTime); // moves the GameObject X and Y value in the given direction multiplied by speed value

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) // If Space bar is pressed and character IS grounded
        {
            //Debug.Log("Jump");
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity); // Increase the players y velocity by Square root of jump height *-2 * gravity. ( Formula Taken from Unity Documentation )
        }

        velocity.y += gravity * Time.deltaTime; // Allows character position y to be manipulated by gravity

        controller.Move(velocity * Time.deltaTime); // // moves the GameObject in all XYZ values in the given direction 

        // Player dies when health reaches 0
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;  
    }
    public void increaseHealth(float healthAmount)
    {
        health += healthAmount;

        if (health >= maxHealth) // Sets maximum health to be 100
        {
            health = maxHealth;
        }
    }
}
