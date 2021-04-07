using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sources used:
//https://learn.unity.com/tutorial/controlling-animation#5c7f8528edbc2a002053b4e0

public class SwordCombat : MonoBehaviour
{
    Animator anim;
    public float damage;
    public float attackRange;
    public float attackSpeed;
    public float attackCooldown;
    public Transform playerCamera;
    public RaycastHit hitInfo;
    public Transform player;

    Enemy enemy;
    BossEnemy boss;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera = player.GetChild(2).transform;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set the sword properties each frame so it is consistent throughout.
        float damage = 10f;
        float attackRange = 25f;
        float attackSpeed = 2f;
        float attackCooldown = 2f;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera = player.GetChild(2).transform;
        anim = GetComponent<Animator>();

        // If left click of mouse is pressed and attack cooldown has passed.
        if (Input.GetMouseButtonDown(0) && Time.time >= attackCooldown)
        {
            attackCooldown = Time.time + 1 / attackSpeed;
            anim.SetBool("attacking", true);
            Attack();
        }
        // If left click is not being pressed.
        else if (Input.GetMouseButtonUp(0))
        {
            anim.SetBool("attacking", false);
        }
    }
    private void Attack()
    {
        RaycastHit hit;

        var layerMask = 1 << 11; // Bit shifts the index of layer 11 (Player layer) to get bit mask

        layerMask = ~layerMask; // We invert it using the ~ sign so that we can collide with everything EXCEPT Layer 11 which is the player.
                                // This is so that we don't shoot ourselves.
        if (Physics.Raycast(playerCamera.transform.position,playerCamera.transform.forward, out hit, attackRange, layerMask))
        {
            if (hit.collider.tag == "Enemy")
            {
                Debug.Log("OUCH");
                enemy = hit.collider.transform.GetComponent<Enemy>();
                enemy.TakeDamage(damage);
            }
            if (hit.collider.tag == "Boss")
            {
                Debug.Log("OUCH");
                boss = hit.collider.transform.GetComponent<BossEnemy>();
                boss.TakeDamage(damage);
            }
        }
    }

}
