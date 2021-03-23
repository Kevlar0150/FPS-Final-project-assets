using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sources used:
//https://learn.unity.com/tutorial/controlling-animation for sword animation

public class PickupSword : MonoBehaviour
{
    // Start is called before the first frame update

    public SwordCombat swordScript;
    public Animator animator;
    public Rigidbody rb;
    public BoxCollider collision;
    public Transform player, meleeSlot, playerCamera;

    public float pickUpRange;

    public bool isEquipped;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera = player.GetChild(2).transform;
        meleeSlot = player.GetChild(2).GetChild(2).transform;

        if (!isEquipped) // If not equipped, disable components attached to object.
        {
            swordScript.enabled = false;
            animator.enabled = false;
            rb.isKinematic = false;
            collision.isTrigger = false;
        }
        if (isEquipped) // If equipped, enable components attached to object.
        {
            swordScript.enabled = true;
            animator.enabled = true;
            rb.isKinematic = true;
            collision.isTrigger = true;
        }
    }
    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera = player.GetChild(2).transform;
        meleeSlot = player.GetChild(2).GetChild(2).transform;

        Vector3 distanceToPlayer = player.position - transform.position; // Distance between player and object.

        // If weapon is not equipped AND weapon is close enough AND 'M' key has been pressed AND meleeSlot is empty AND object being picked has tag "Sword"
        if (!isEquipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.M) && meleeSlot.childCount <= 0 && transform.tag == "Sword")
        {
            PickUp();
        }
        // If weapon is equipped and G is pressed, drop weapon.
        if (isEquipped && Input.GetKeyDown(KeyCode.G))
        {
            Drop();
        }
    }

    private void PickUp()
    {
        // If melee slot is empty 
        if (meleeSlot.childCount <= 0)
        {
            isEquipped = true;

            rb.isKinematic = true;
            collision.isTrigger = true;

            //Assigns MeleeSlot gameobject as parent and reset swords transform.
            transform.SetParent(meleeSlot);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.Euler(Vector3.zero);

            swordScript.enabled = true;
            animator.enabled = true;
        }
    }
    private void Drop()
    {
        isEquipped = false;

        // Change transform to its own transform manipulated by engine physics.
        transform.SetParent(null);

        rb.isKinematic = false;
        collision.isTrigger = false;

        swordScript.enabled = false;
        animator.enabled = false;
    }
}
