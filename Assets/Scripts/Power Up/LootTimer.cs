﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTimer : MonoBehaviour
{
    public float lifeDuration = 5f;
    private float lifeTimer;
    private bool hitGround = false;
    // Start is called before the first frame update
    void Start()
    {
        lifeTimer = lifeDuration += Random.Range(5f, 9f);
    }

    // Update is called once per frame
    void Update()
    {
        if (hitGround)
        {
            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0)
            {
                Debug.Log(gameObject + "Destroyed");
                DestroyObject(gameObject);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            hitGround = true;
        }
    }
}