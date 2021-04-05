using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCPatrol : MonoBehaviour
{
    // BossEnemy health
    public float enemyHealth = 50f;

    // Has the agent wait at a node for specified amount of time.
    public bool patrolWaiting;
    public float timeWaiting = 2f;

    // List of waypoints the NPC will visit
    public List<Waypoints> waypointList;

    // Attack variables
    public GameObject projectilePrefab;
    public GameObject muzzle;
    public float timeBetweenAttacks;
    bool hasAttacked;

    // Sight Range variables
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    // Control death of enemy
    private float lifeDuration = 3f;
    private float lifeTimer;
    private bool hasDied = false;

    // Layermasks
    public LayerMask Ground, Player;

    // Variables for patrol behaviour
    NavMeshAgent navMeshAgent;
    public int currentWaypointIndex;
    public bool moving;
    public  bool waiting;
    public bool moveForward;
    public float waitTimer;

    //References
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        lifeTimer = lifeDuration;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (navMeshAgent == null)
        {
            Debug.LogError("Nav mesh agent component is not attached to " + gameObject.name);
        }

        else
        {
            if (waypointList != null && waypointList.Count >= 2)
            {
                currentWaypointIndex = 0;
                SetDestination();
            }
            else
            {
                Debug.LogError("Not enough patrol points");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 distanceToWalkPoint = transform.position - waypointList[currentWaypointIndex].transform.position;

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
                //SetDestination();
                Patrol(distanceToWalkPoint); // Patrol between points
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

    private void Patrol(Vector3 distanceToWalkPoint)
    {
        if (moving && distanceToWalkPoint.magnitude <= 1.0f)
        {
            moving = false;

            if (patrolWaiting)
            {
                Debug.Log("Waiting");
                waiting = true;
                waitTimer = 0f;
            }
            else
            {
                ChangeWaypoint();
                SetDestination();
            }
        }

        if (waiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= timeWaiting)
            {
                waiting = false;

                ChangeWaypoint();
                SetDestination();
            }
        }
    }

    private void ChangeWaypoint() // Change to another waypoint available in list with probability to go backwards in list.
    {
        currentWaypointIndex = Random.Range(0, waypointList.Count);
    }

    private void SetDestination() // Set new waypoint for NPC to travel to
    {
        if (waypointList != null)
        {
            Vector3 targetVector = waypointList[currentWaypointIndex].transform.position;
            navMeshAgent.SetDestination(targetVector);
            moving = true;
        }
    }

    private void Chase()
    {
        // Debug.Log("Chasing");
        navMeshAgent.SetDestination(player.position);
    }

    private void Attack()
    {
        //Debug.Log("Attacking");
        navMeshAgent.SetDestination(transform.position);
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
            transform.GetChild(0).gameObject.SetActive(false); // Gets the child of the object which in this case is the BossEnemy mesh and DISABLE IT
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
