using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Entire script has been produced 100% by me.
public class SwordSwing : MonoBehaviour
{
    bool hasHit = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        BossEnemy boss = other.GetComponent<BossEnemy>();

        if (enemy && hasHit == false)
        {
            enemy.TakeDamage(20);
            hasHit = true;
        }
        if (boss && hasHit == false)
        {
            boss.TakeDamage(20);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        hasHit = false;
    }
}
