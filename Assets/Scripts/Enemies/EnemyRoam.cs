using UnityEngine;
using System.Collections;

// Entire code taken from 1 of the answers by "JustingReinhart",(2013) on Unity Forum, https://forum.unity.com/threads/randomly-generate-objects-inside-of-a-box.95088/. 
// Not used in the final build of the project due to bugs and issues with using this method but have left the script inside incase I need to refer back to it.
public class EnemyRoam : MonoBehaviour
{

    public GameObject ObjectToSpawn;
    public float RateOfSpawn = 1;

    private float nextSpawn = 0;

    BoxCollider boxCollider;
    Bounds bound;

    // Update is called once per frame
    void Update()
    {
        boxCollider = GetComponent<BoxCollider>();
        bound = boxCollider.bounds;
        if (Time.time > nextSpawn)
        {
            nextSpawn = Time.time + RateOfSpawn;

            // Random position within this transform
            Vector3 rndPosWithin;         
            rndPosWithin = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
            rndPosWithin = transform.TransformPoint(rndPosWithin * 5f);
            Instantiate(ObjectToSpawn, rndPosWithin, transform.rotation,gameObject.transform);
        }
    }
}