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
        for (int i = 0; i < transformArray.Length; i++)
        {
            int itemRandomNumber = Random.Range(0, 3); // Random number to select category of items

            if (itemRandomNumber == 0) // Selects gun category
            {
                int gunNumber = Random.Range(0, gunArray.Length);
                Instantiate(gunArray[gunNumber], transformArray[i]);
            }

            if (itemRandomNumber == 1) // Selects Power up category
            {
                int powerUpNumber = Random.Range(0, lootArray.Length);
                Instantiate(lootArray[powerUpNumber], transformArray[i]);
            }

            if (itemRandomNumber == 2) // Select Energy cannon
            {
                Instantiate(energyCannonSpawn, transformArray[i]);
            }

            if (itemRandomNumber == 3) // Select sword
            {
                Instantiate(swordSpawn, transformArray[i]);
            }
        }        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
