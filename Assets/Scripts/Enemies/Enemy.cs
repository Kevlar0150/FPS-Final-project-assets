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
    // Enemy health
    public float enemyHealth = 50f;

    // Has the agent wait at a node for specified amount of time.
    public bool patrolWaiting;
    public float timeWaiting = 2f;

    // List of waypoints the NPC will visit
    public List<Transform> waypointList;

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
    public bool patrolling;
    public bool waiting;
    public bool moveForward;
    public float waitTimer;

    //References
    [SerializeField]Transform player;
    Transform EnemyParent;
    Transform PatrolPoints;
    Animator anim;

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        //Initialising variables
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        lifeTimer = lifeDuration;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        NavMeshPath path = new NavMeshPath();

        //Add patrol points into list
        EnemyParent = transform.parent;
        Debug.Log(PatrolPoints = EnemyParent.GetChild(0));
        PatrolPoints = EnemyParent.GetChild(0);

        // Foreach child transform in PatrolPoints Gameobject, add child to list
        foreach (Transform child in PatrolPoints)
        {
            waypointList.Add(child);
        }

        if (navMeshAgent == null)// If navMeshAgent component not found
        { Debug.LogError("Nav mesh agent component is not attached to " + gameObject.name); }

        else
        {
            if (waypointList != null && waypointList.Count >= 2) // If waypoint list has AT LEAST 2 elements.
            {
                Debug.Log("PATROL");
                currentWaypointIndex = 0;
                Invoke("SetDestination",0);
            }
            else
            { Debug.LogError("Not enough patrol points"); } // Else output an error
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
            // Creates spheres around NPC to act as detection mechanism
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, Player);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, Player);

            if (!playerInSightRange && !playerInAttackRange) // If player is NOT in sight or attack range
            {
                if (!patrolling && !waiting) // If NPC has stopped chasing/attacking, call functions to change destination.
                {
                    ChangeWaypoint();
                    SetDestination();

                   
                }

                Patrol(distanceToWalkPoint); // Patrol between points
            }
            if (playerInSightRange && !playerInAttackRange)
            {
                patrolling = false;
                Chase(); // Chases player until player is in attack range
            }
            if (playerInSightRange && playerInAttackRange)
            {
                patrolling = false;
                Attack(); // Attacks player when in attack range
            }
        }
    }

    private void Patrol(Vector3 distanceToWalkPoint)
    {
        Debug.Log("Patrolling");
        anim.SetBool("isWalking", true);
        anim.SetBool("isAttacking", false);

        if (patrolling && distanceToWalkPoint.x <= 3.0f && distanceToWalkPoint.z <=3.0f) // If patrolling AND destination has been reached
        {
            patrolling = false; // NPC stops patrolling

            if (patrolWaiting) // If NPC has waiting feature enabled
            {
                //Debug.Log("Waiting");
                waiting = true;
                waitTimer = 0f;
            }
            else // If waiting feature DISABLED, just change waypoint once desination reached.
            {
                ChangeWaypoint();
                SetDestination();
            }
        }
   
        if (waiting)     // If NPC is waiting
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isAttacking", false);
            waitTimer += Time.deltaTime; // Increase waitTimer

            if (waitTimer >= timeWaiting) // If waitTimer has reached the specified time to wait then...
            {
                waiting = false; // No longer waiting
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
            patrolling = true;
            anim.SetBool("isWalking", true);
            anim.SetBool("isAttacking", false);
        }
    }
    private void Chase()
    {
        anim.SetBool("isWalking", true);
        anim.SetBool("isAttacking", false);
        Debug.Log("Chasing");
        navMeshAgent.SetDestination(player.position);
    }

    private void Attack()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", true);
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
    public void TakeDamage(float damage) // Function to handle enemy taking damage
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
