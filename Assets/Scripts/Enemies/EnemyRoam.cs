using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoam : MonoBehaviour
{
    public GameObject cube;

    BoxCollider boxCollider;
    Bounds bound;
    public int loops = 5;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        bound = boxCollider.bounds;
        InvokeRepeating("spawnPoint", 5, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnPoint()
    {
        Debug.Log("SPAWN CUBE");
        var cubeVariable = Instantiate(cube, RandomPointInBounds(bound), Quaternion.identity);
        cubeVariable.transform.parent = gameObject.transform;
        cubeVariable.transform.localPosition = boxCollider.transform.localPosition;
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
