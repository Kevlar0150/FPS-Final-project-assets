using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy[] enemyArray; // Array of enemies to be spawned

    public Transform[] transformArray; // Array of transforms specifying location to spawn the enemies.

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
            for (int i = 0; i <= transformArray.Length; i++)
            {
                Instantiate(enemyArray[Random.Range(0,enemyArray.Length)], transformArray[i]);
                hasEnemySpawned = true;
            }
        }
    }
}
