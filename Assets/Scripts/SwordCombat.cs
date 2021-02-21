using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sources used:
//https://learn.unity.com/tutorial/controlling-animation#5c7f8528edbc2a002053b4e0

public class SwordCombat : MonoBehaviour
{
    Animator anim;
    public float damage = 50f;
    public float attackRange = 25f;
    public float attackSpeed;
    public float attackCooldown = 0f;
    public Camera playerCamera;
    public RaycastHit hitInfo;

    Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= attackCooldown)
        {
            attackCooldown = Time.time + 1 / attackSpeed;
            anim.SetBool("attacking", true);
            Attack();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            anim.SetBool("attacking", false);
        }
    }
    // TO DO:
    // ADD ATTACK USING RAY CAST
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
        }
    }

}
