﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyProjectile : MonoBehaviour
{
    // Projectile properties
    public float speed = 1f;
    public float lifeDuration = 3f;
    public float spread;
    public float damage = 10;

    private float lifeTimer;
    public GameObject impactVFX;

    // Start is called before the first frame update
    void Start()
    {
        lifeTimer = lifeDuration;
    }

    // Update is called once per frame
    void Update()
    {
        // Make bullet move forward
        transform.position += transform.forward;

        //If statement for destroying bullet to save memory.
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            DestroyObject(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Impact VFX when projectil collides with anything
        GameObject impactVFXObject = Instantiate(impactVFX, transform.position, transform.rotation);
        DestroyObject(gameObject);
        Destroy(impactVFXObject, 1.7f);

        // Damage player
        Player playerObject = collision.transform.GetComponent<Player>(); // Gets Player component when collides Player - Allows us to call functions in Player script.
        if (playerObject != null) // If playerObject HAS FOUND the Player component and does not equal NULL
        {
            playerObject.TakeDamage(damage); 
        }
    }
}
