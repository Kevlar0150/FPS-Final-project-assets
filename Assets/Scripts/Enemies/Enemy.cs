using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Online resources used:
//https://docs.unity3d.com/Manual/class-NavMeshAgent.html
//https://docs.unity3d.com/Manual/nav-CreateNavMeshAgent.html
//https://docs.unity3d.com/Manual/class-NavMeshSurface.html

public class Enemy : MonoBehaviour
{
    public float enemyHealth = 50f;

    public NavMeshAgent agent;

    public Transform player;

    public LayerMask Ground, Player;

    // Patrol variables
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attack variables
    public GameObject projectilePrefab;
    public GameObject muzzle;
    public float timeBetweenAttacks;
    bool hasAttacked;

    // Range variables
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private float lifeDuration = 3f;
    private float lifeTimer;
    private bool hasDied = false;

    // Start is called before the first frame update
    void Start()
    {
        lifeTimer = lifeDuration;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasDied) // If enemy has died
        {
            lifeTimer -= Time.deltaTime; // Start timer

            if (lifeTimer <= 0) // When timer finished, destroy enemy Game Object
            {
                DestroyObject(gameObject); // Destroys the object that this script is attached too that has enemy health <= 0
            }
        }
        if (!hasDied) // If enemy is alive
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, Player);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, Player);

            if (!playerInSightRange && !playerInAttackRange)
            {
                Patrol(); // Patrols between points
            }
            if (playerInSightRange && !playerInAttackRange)
            {
                Chase(); // Chases player until player is in attack range
            }
            if (playerInSightRange && playerInAttackRange)
            {
                Attack(); // Attacks player when in attack range
            }
        }
    }

    private void Patrol()
    {
        if (!walkPointSet) // If walkPointSet is false
        { 
            SearchWalkPoint(); // Set the walkpoint
        }
        if (walkPointSet) // If walkPointSet is true
        {
            agent.SetDestination(walkPoint);  // make the walkPoint the enemies destination     
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1.0f) // If walkPoint destination has been reached then set walkPointSet to false
        {
            walkPointSet = false;
        }
    }
    private void SearchWalkPoint()
    {
        // Debug.Log("Searching for point");
        // Calculate random points in range
        
        // TODO LATER WHEN PUTTING LEVEL AND ENEMYS TOGETHER:
        // MAKE IT SO IT ROAMS AROUND  depending on the room it's in and the size of that room. 

        // Temporarily set walkpoint range to be fixed range. Only works with the TestArena
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        //float randomY = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(randomX, transform.position.y,randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 20f, Ground))
        {
            walkPointSet = true;
        }
    }
    private void Chase()
    {
       // Debug.Log("Chasing");
        agent.SetDestination(player.position);
    }
    private void Attack()
    {
        //Debug.Log("Attacking");
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!hasAttacked) // If enemy hasn't attacked yet, then ATTACK
        {
            GameObject enemyProjectile = Instantiate(projectilePrefab);
            enemyProjectile.transform.position = muzzle.transform.position;
            enemyProjectile.transform.forward = gameObject.transform.forward;
  
            hasAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks); // Invoke resetAttack function after timeBetweenAttacks time has ended
        }
    }
    private void ResetAttack() // Sets bool "hasAttacked" to false;
    {
        hasAttacked = false;
    }
    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;

        if (enemyHealth <= 0)
        {
            hasDied = true; // Set hasDied to true and start timer to destroy the gameObject.
            transform.GetChild(0).gameObject.SetActive(false); // Gets the child of the object which in this case is the Enemy mesh and DISABLE IT
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<SpawnLoot>().setSpawnLoot(true); // Call function to spawn loot.
        }
    }

    // Draws radius of attackRange and sightRange for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
