using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public PowerUps[] lootArray; // Array of power ups that can be picked up

    public Gun_raycast[] gunArray;// Array of guns that can be equipped

    public EnergyCannon energyCannonSpawn; // A prefab of energyGun to be spawned in the room

    public SwordCombat swordSpawn; // A prefab of sword to be spawned in the room

    public Transform[] transformArray; // Array of transforms where items will be spawned.

    // Start is called before the first frame update
    void Start()
    {
        // Randomly select the item from the array
        // Spawn at transform specified.

        Instantiate(gunArray[0], transformArray[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
