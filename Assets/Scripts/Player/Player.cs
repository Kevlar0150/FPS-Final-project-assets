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
    private float maxSpeedIncrease = 15;

    //Bools
    bool speedBuffOn = false;
    bool hit = false;

    private void Start()
    {
        speedBoostTimer = speedBoostDuration;
        speed = defaultSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        // ---------------------- Player Movement -----------------------

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
