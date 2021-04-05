using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public BossEnemy[] bossArray; // Array of enemies to be spawned

    public Transform[] transformArray; // Array of transforms specifying location to spawn the enemies.

    LevelBuilder levelBuilder;

    bool hasBossSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        hasBossSpawned = false;
        levelBuilder = GetComponentInParent<LevelBuilder>();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelBuilder.hasRoomFinished() == true && hasBossSpawned == false) // If level structure has finished generating, then start spawning 
        {
            Debug.Log("ROOM FINISHED GENERATING");
            for (int i = 0; i <= transformArray.Length; i++)
            {
                Instantiate(bossArray[Random.Range(0,3)], transformArray[i]);
                hasBossSpawned = true;
            }
        }
    }
}
