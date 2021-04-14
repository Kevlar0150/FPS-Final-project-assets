using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public PowerUps[] lootArray; // Array of power ups that can be picked up

    public List<Gun_raycast> gunList = new List<Gun_raycast>();

    public EnergyCannon energyCannonSpawn; // A prefab of energyGun to be spawned in the room

    public SwordCombat swordSpawn; // A prefab of sword to be spawned in the room

    public Transform[] transformArray; // Array of transforms where items will be spawned.

    public bool gunsOnly = false;
    public bool powerupsOnly = false;
    public bool swordOnly = false;
    public bool timedLoot = false;

    private bool energyGunSpawned = false;

    public float spawnTimer = 15.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Randomly select the item from the array
        // Spawn at transform specified.

        //GUNS ONLY
        if (gunsOnly && !powerupsOnly && !swordOnly)
        {
            for (int i = 0; i < transformArray.Length; i++)
            {
                int itemRandomNumber = Random.Range(0, 1); // Random number to select category of items

                if (itemRandomNumber == 0) // Selects gun category
                {
                    int gunNumber = Random.Range(0, gunList.Count);
                    Instantiate(gunList[gunNumber], transformArray[i]);
                    gunList.RemoveAt(gunNumber);                    
                }

                if (itemRandomNumber == 1 && gunsOnly && !energyGunSpawned) // Select Energy cannon
                {
                    Instantiate(energyCannonSpawn, transformArray[i]);
                    energyGunSpawned = true;
                }
            }
        }

        //SWORD ONLY
        if (swordOnly && !powerupsOnly && !gunsOnly)
        {
            Instantiate(swordSpawn, transformArray[Random.Range(0, transformArray.Length)]);
        }

      
        //POWER UPS ONLY
        SpawnPowerUps();

        //ALL ITEMS 
        if (powerupsOnly && gunsOnly && swordOnly)
        {
            for (int i = 0; i < transformArray.Length; i++)
            {
                int itemRandomNumber = Random.Range(0, 4); // Random number to select category of items

                if (itemRandomNumber == 0) // Selects gun category
                {
                    int gunNumber = Random.Range(0, gunList.Count);
                    Instantiate(gunList[gunNumber], transformArray[i]);
                    gunList.RemoveAt(gunNumber);
                }

                if (itemRandomNumber == 1 && !energyGunSpawned) // Select Energy cannon
                {
                    Instantiate(energyCannonSpawn, transformArray[i]);
                    energyGunSpawned = true;
                }

                if (itemRandomNumber == 2) // Select sword
                {
                    Instantiate(swordSpawn, transformArray[i]);
                }

                if (itemRandomNumber == 3) // Selects powerUp category
                {
                    Debug.Log("SPAWN LOOT");
                    int powerupNumber = Random.Range(0, lootArray.Length);
                    Instantiate(lootArray[powerupNumber], transformArray[i]);
                }
            }
        }

        //POWER UP AND GUN
        if (powerupsOnly && gunsOnly && !swordOnly)
        {
            Debug.Log("Guns and Loot");
            for (int i = 0; i < transformArray.Length; i++)
            {
                int itemRandomNumber = Random.Range(0, 3); // Random number to select category of items

                if (itemRandomNumber == 0) // Selects gun category
                {
                    int gunNumber = Random.Range(0, gunList.Count);
                    Instantiate(gunList[gunNumber], transformArray[i]);
                    gunList.RemoveAt(gunNumber);
                }

                if (itemRandomNumber == 1 && !energyGunSpawned) // Select Energy cannon
                {
                    Instantiate(energyCannonSpawn, transformArray[i]);
                    energyGunSpawned = true;
                }
                if (itemRandomNumber == 2) // Selects powerUp category
                {
                    Debug.Log("SPAWN LOOT");
                    int powerupNumber = Random.Range(0, lootArray.Length);
                    Instantiate(lootArray[powerupNumber], transformArray[i]);
                }
            }
        }

        //SWORD AND GUN
        if (gunsOnly && !powerupsOnly && swordOnly)
        {
            for (int i = 0; i < transformArray.Length; i++)
            {
                int itemRandomNumber = Random.Range(0, 2); // Random number to select category of items

                if (itemRandomNumber == 0) // Selects gun category
                {
                    int gunNumber = Random.Range(0, gunList.Count);
                    Instantiate(gunList[gunNumber], transformArray[i]);
                    gunList.RemoveAt(gunNumber);
                }

                if (itemRandomNumber == 1 && gunsOnly && !energyGunSpawned) // Select Energy cannon
                {
                    Instantiate(energyCannonSpawn, transformArray[i]);
                    energyGunSpawned = true;
                }
                if (itemRandomNumber == 2) // Select sword
                {
                    Instantiate(swordSpawn, transformArray[i]);
                }
            }
        }

        //SWORD AND POWER UP
        if (powerupsOnly && !gunsOnly && swordOnly)
        {
            Debug.Log("Guns and Loot");
            for (int i = 0; i < transformArray.Length; i++)
            {
                int itemRandomNumber = Random.Range(0, 2); // Random number to select category of items


                if (itemRandomNumber == 0) // Select sword
                {
                    Instantiate(swordSpawn, transformArray[i]);
                }
                if (itemRandomNumber == 1) // Selects powerUp category
                {
                    Debug.Log("SPAWN LOOT");
                    int powerupNumber = Random.Range(0, lootArray.Length);
                    Instantiate(lootArray[powerupNumber], transformArray[i]);
                }
            }
        }
    }

    private void SpawnPowerUps()
    {
        if (powerupsOnly && !gunsOnly && !swordOnly)
        {
            for (int i = 0; i < transformArray.Length; i++)
            {
                int powerupNumber = Random.Range(0, lootArray.Length);
                Instantiate(lootArray[powerupNumber], transformArray[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer-= Time.deltaTime;
        if (timedLoot)
        {
            if (spawnTimer <= 0)
            {
                SpawnPowerUps();
                spawnTimer = 15;
            }
        }        
    }
}
