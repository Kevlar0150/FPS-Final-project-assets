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
        //spawnPoint.position = GetComponent<Enemy>().getLastPosition();
        if (minNumberOfLoot > maxNumberOfLoot)
        {
            maxNumberOfLoot = minNumberOfLoot + 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnLoot && !hasBeenCollected)
        {
            spawnLoot = false;
            Loot();
        }
    }

    public void Loot()
    {
        hasBeenCollected = true;
        int number = Random.Range(minNumberOfLoot, maxNumberOfLoot);
        StartCoroutine(CreateLoot(number));
    }

    IEnumerator CreateLoot(int number)
    {
        //this.GetComponent<Animator>().SetTrigger("OpenChest");
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < number; i++)
        {
            GameObject tempLoot = Instantiate(lootToBeSpawned[Random.Range(0, lootToBeSpawned.Count)]);
            tempLoot.transform.position = spawnPoint.position;
            yield return new WaitForSeconds(0.15f);
        }
    }
    public void setSpawnLoot(bool isSpawned)
    {
        Debug.Log("SPAWN LOOT");
        spawnLoot = isSpawned;
    }
}

