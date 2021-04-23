
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Online resources used:
//https://docs.unity3d.com/Manual/class-NavMeshAgent.html
//https://docs.unity3d.com/Manual/nav-CreateNavMeshAgent.html
//https://docs.unity3d.com/Manual/class-NavMeshSurface.html

public class BossEnemy : MonoBehaviour
{
    // BossEnemy health
    public float enemyHealth = 50f;
    private float enemyMaxHealth;
    private bool isEnraged = false;

    // Has the agent wait at a node for specified amount of time.
    public bool patrolWaiting;
    public float timeWaiting = 2f;

    // List of waypoints the NPC will visit
    public List<Transform> waypointList;

    // Attack variables
    public float timeBetweenAttacks;
    bool hasAttacked;

    // Shooting variables
    public GameObject projectilePrefab;
    public GameObject muzzle;
    public float timeBetweenShots;
    public float numOfProjectilesShot;
    bool hasShot;

    // Sight Range variables
    public float sightRange, attackRange, shootRange;
    public bool playerInSightRange, playerInAttackRange, playerInShootRange;

    // Control death of enemy
    public float lifeDuration = 3f;
    public float lifeTimer;
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
    [SerializeField] Transform player;
    Collider rightLeg;
    Transform enemyParent;
    Transform patrolPoints;
    Animator anim;

    //VFX Variables
    public Transform armLeft;
    public Transform armRight;

    GameObject flameVFXObject;
    GameObject smokeVFXObject;
    GameObject deathVFXObject;
    GameObject deathVFXObject2;

    public GameObject deathExplosion;
    public GameObject deathExplosion2;
    public GameObject flame;
    public GameObject smoke;

    //Ref to mainmenu script
    private MainMenu mainMenu;

    //Difficulty multiplier
    float difficultyMultiplier;

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        //Initialising variables
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        lifeTimer = lifeDuration;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rightLeg = gameObject.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();
        NavMeshPath path = new NavMeshPath();

        mainMenu = GameObject.Find("MenuManager").GetComponent<MainMenu>();
        difficultyMultiplier = mainMenu.getMultiplier();

        //Setting the boss properties values
        enemyHealth *= difficultyMultiplier;
        sightRange *= difficultyMultiplier;
        attackRange *= difficultyMultiplier;
        shootRange *= difficultyMultiplier;
        navMeshAgent.speed *= difficultyMultiplier;
        navMeshAgent.acceleration *= difficultyMultiplier;
        numOfProjectilesShot *= difficultyMultiplier;

        //Add patrol points into list
        enemyParent = transform.parent;
        Debug.Log(patrolPoints = enemyParent.GetChild(0));
        patrolPoints = enemyParent.GetChild(0);

        enemyMaxHealth = enemyHealth;

        // Foreach child transform in PatrolPoints Gameobject, add child to list
        foreach (Transform child in patrolPoints)
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
                Invoke("SetDestination", 0);
            }
            else
            { Debug.LogError("Not enough patrol points"); } // Else output an error
        }
    }

    // Update is called once per frame
    void Update()
    {
        rightLeg.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 distanceToWalkPoint = transform.position - waypointList[currentWaypointIndex].transform.position;       

        if (!hasDied) // If enemy is alive
        {
            // Creates spheres around NPC to act as detection mechanism
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, Player);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, Player);
            playerInShootRange = Physics.CheckSphere(transform.position, shootRange, Player);

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
            if (playerInSightRange && playerInAttackRange )
            {
                
                patrolling = false;
                Attack(); // Attacks player when in attack range
            }

            if (playerInSightRange && playerInShootRange && !playerInAttackRange)
            {
                patrolling = false;
                Shoot(); // shoot player when in attack range
            }

            // If health below threshold, buff the enemy
            if (enemyHealth <= (0.4*enemyMaxHealth))
            {
                Enrage();
             
            }
        }
    }

    private void Patrol(Vector3 distanceToWalkPoint)
    {

        if (patrolling && distanceToWalkPoint.x <= 1.0f && distanceToWalkPoint.z <= 1.0f) // If patrolling AND destination has been reached
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

        if (waiting) // If NPC is waiting
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
        rightLeg.enabled = true;
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", true);
        navMeshAgent.SetDestination(transform.position);
        navMeshAgent.transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        if (!hasAttacked)
        {
                    
            hasAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks); // Invoke resetAttack function after timeBetweenAttacks time has ended
        }
    }

    private void ResetAttack() // Sets bool "hasAttacked" to false;
    {
        hasAttacked = false;
        anim.SetBool("isAttacking", false);
    }

    private void Shoot()
    {
        anim.SetBool("isWalking", false);

        navMeshAgent.SetDestination(transform.position);
        navMeshAgent.transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        //muzzle.transform.LookAt(player);

        if (!hasShot) // If enemy hasn't shot yet, then SHOOT
        {

            // LOOPS 8 times to create 8 instances of the bullet when Shoot function called
            for (int i = 0; i < numOfProjectilesShot; i++)
            {
                GameObject enemyProjectile = Instantiate(projectilePrefab);
                enemyProjectile.transform.position = muzzle.transform.position;
                enemyProjectile.transform.forward = muzzle.transform.forward;
            }

            hasShot= true;
            Invoke(nameof(ResetShot), timeBetweenShots); // Invoke resetAttack function after timeBetweenAttacks time has ended
        }
    }

    private void ResetShot()
    {
        hasShot = false;
    }
  
    public void TakeDamage(float damage) // Function to handle enemy taking damage
    {
        enemyHealth -= damage;

        if (enemyHealth <= 0)
        {
            deathVFXObject = Instantiate(deathExplosion, transform.position, transform.rotation);
            Destroy(deathVFXObject, 1.7f);

            deathVFXObject2 = Instantiate(deathExplosion2, transform.position + new Vector3(0,2,0), transform.rotation);
            Destroy(deathVFXObject2);

            hasDied = true;
            transform.GetChild(2).gameObject.SetActive(false); // Gets the child of the object which in this case is the BossEnemy mesh and DISABLE IT
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<SpawnLoot>().setSpawnLoot(true); // Call function to spawn loot.
            Invoke("DestroyObject", 3);
        }
    }

    public void Enrage()
    {
        // Turn on shutdown and startup animation;
        Debug.Log("enraged");
        if (!isEnraged)
        {

            navMeshAgent.speed *= 2f;
            navMeshAgent.angularSpeed = 200f;
            numOfProjectilesShot = 20;
            sightRange *= 2f;
            shootRange *= 2f;
            isEnraged = true;
        }       
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
    // Draws radius of attackRange and sightRange for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }
}
