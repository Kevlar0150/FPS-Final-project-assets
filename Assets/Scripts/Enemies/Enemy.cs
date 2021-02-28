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
    public float timeBetweekAttacks;
    bool hasAttacked;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
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

    private void Patrol()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);        
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1.0f)
        {
            walkPointSet = false;
        }
    }
    private void SearchWalkPoint()
    {
        //Debug.Log("Searching for point");
        //Calculate random points in range
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        //float randomY = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 10f, Ground))
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

        if (!hasAttacked)
        {
        GameObject enemyProjectile = Instantiate(projectilePrefab);
        enemyProjectile.transform.position = muzzle.transform.position;
        enemyProjectile.transform.forward = gameObject.transform.forward;
  
        hasAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweekAttacks);
        }
    }
    private void ResetAttack()
    {
        hasAttacked = false;
    }
    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;

        if (enemyHealth <= 0)
        {
            DestroyObject(gameObject); // Destroys the object that this script is attached too that has enemy health <= 0
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
