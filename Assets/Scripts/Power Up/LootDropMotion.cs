﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Entire code present in the script has been produced following a tutorial from OneWheelStudio, 2019, https://www.youtube.com/watch?v=lqGlBMe-Pqw
public class LootDropMotion : MonoBehaviour
{
    private Vector3 velocity = Vector3.up;
    private Rigidbody rb;
    private Vector3 startPosition;
    public bool hitGround = false;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.transform.position;
        velocity *= Random.Range(4f, 6f);
        velocity += new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1, 1));

        rb = this.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Speed at which loot propels upwards
        rb.position += velocity * 1 * Time.deltaTime;

        // Rotate the object randomly
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(Random.Range(-150f, 150f), Random.Range(-200, 200), Random.Range(-150, 150)) * 2 *  Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);

        if (velocity.y < -4f)
        {
            velocity.y = -4f;
        }
        else
        {
            // Speed at which loot falls
            velocity -= Vector3.up * 4 * Time.deltaTime;
        }

        if (hitGround == true)
        {
            Debug.Log("Hit the ground");

            //Enable and disable RB properties so that Unity can handle the physics when hitting the ground
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            this.enabled = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
            hitGround = true;
    }
}

