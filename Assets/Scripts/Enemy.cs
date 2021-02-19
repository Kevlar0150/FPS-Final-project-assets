using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float enemyHealth = 50f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;

        if (enemyHealth <= 0)
        {
            DestroyObject(gameObject); // Destroys the object that this script is attached too that has enemy health <= 0
        }
    }
}
