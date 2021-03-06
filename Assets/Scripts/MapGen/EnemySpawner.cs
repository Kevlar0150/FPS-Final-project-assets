﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Entire code presented in this script has been created 100% by me

public class EnemySpawner : MonoBehaviour
{
    public Enemy[] enemyArray; // Array of enemies to be spawned

    public Transform[] spawnPositions; // Array of transforms specifying location to spawn the enemies.

    LevelBuilder levelBuilder;

    bool hasEnemySpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        hasEnemySpawned = false;
        levelBuilder = GetComponentInParent<LevelBuilder>();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelBuilder.hasRoomFinished() == true && hasEnemySpawned == false) // If level structure has finished generating, then start spawning 
        {
            Debug.Log("ROOM FINISHED GENERATING");
            for (int i = 0; i <= spawnPositions.Length; i++)
            {
                Instantiate(enemyArray[Random.Range(0,enemyArray.Length)], spawnPositions[i]);
                hasEnemySpawned = true;
            }
        }
    }
}
