﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    public Gun_raycast gunScript;
    public Rigidbody rb;
    public BoxCollider collision;
    public Transform player, weaponSlot1, weaponSlot2, playerCamera;

    public float pickUpRange;

    public bool isEquipped;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        if (!isEquipped)
        {
            gunScript.enabled = false;
            rb.isKinematic = false;
            collision.isTrigger = false;
            anim.enabled = false;
        }
        if (isEquipped)
        {
            gunScript.enabled = true;
            rb.isKinematic = true;
            collision.isTrigger = true;
            anim.enabled = true;
        }
    }
    private void Update()
    {
        Vector3 distanceToPlayer = player.position - transform.position;
        // Check conditions mets before allowing player to pick up weapon.
        if (!isEquipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && weaponSlot1.childCount <= 0 && transform.tag != "Sword")
        {
            PickUp();
        }
        // Check conditions mets before allowing player to pick up 2ndary weapon.
        if (!isEquipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.Q) && weaponSlot2.childCount <= 0 && transform.tag != "Sword")
        {
            PickUpSecondary();
        }
        // Check conditions mets before allowing player to drop weapon
        if (isEquipped && Input.GetKeyDown(KeyCode.G))
        {
            Drop();
        }
    }

    private void PickUp()
    {
        //If weapon slot is empty
        if (weaponSlot1.childCount <= 0)
        {
            isEquipped = true;

            rb.isKinematic = true;
            collision.isTrigger = true;

            // Set weaponslot 1 as parent and reset transform.
            transform.SetParent(weaponSlot1);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.Euler(Vector3.zero);

            gunScript.enabled = true;
            anim.enabled = true;
        }
    }
    private void PickUpSecondary()
    {
        //If weapon slot is empty
        if (weaponSlot2.childCount <= 0)
        {
            isEquipped = true;

            rb.isKinematic = true;
            collision.isTrigger = true;

            // Set weaponslot 1 as parent and reset transform.
            transform.SetParent(weaponSlot2);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.Euler(Vector3.zero);

            gunScript.enabled = true;
            anim.enabled = true;
        }
    }

    private void Drop()
    {
        isEquipped = false;

        // Change transform to its own transform manipulated by engine physics.
        transform.SetParent(null);

        rb.isKinematic = false;
        collision.isTrigger = false;

        gunScript.enabled = false;
        anim.enabled = false;
    }
}
