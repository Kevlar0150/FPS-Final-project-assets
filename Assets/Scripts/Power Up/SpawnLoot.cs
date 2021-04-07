using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLoot : MonoBehaviour
{
    public List<GameObject> lootToBeSpawned = new List<GameObject>();
    public int minNumberOfLoot = 3;
    public int maxNumberOfLoot = 10;
    public Transform spawnPoint;
    public bool hasBeenCollected = false;

    public bool spawnLoot = false;

    // Start is called before the first frame update
    void Start()
    {
        if (minNumberOfLoot > maxNumberOfLoot)
        {
            maxNumberOfLoot = minNumberOfLoot + 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnLoot && !hasBeenCollected) // If spawnLoot is true and loot has not been collected.
        {
            spawnLoot = false; // Set to false
            Loot(); // Call loot function.
        }
    }

    public void Loot()
    {
        hasBeenCollected = true; // Set hasBeenCollected to true
        int number = Random.Range(minNumberOfLoot, maxNumberOfLoot); // Set number of loot to a random number
        StartCoroutine(CreateLoot(number));
    }

    IEnumerator CreateLoot(int number)
    {
        yield return new WaitForSeconds(1f); // Wait 1 second before continuing the function

        // For loop to instantiate the loot at spawn position.
        for (int i = 0; i < number; i++)
        {
            GameObject tempLoot = Instantiate(lootToBeSpawned[Random.Range(0, lootToBeSpawned.Count)]);
            tempLoot.transform.position = spawnPoint.position;
            yield return new WaitForSeconds(0.15f);
        }
    }
    public void setSpawnLoot(bool isSpawned)
    {
        Debug.Log("SPAWNED LOOT BOSS");
        spawnLoot = isSpawned;
    }
}

