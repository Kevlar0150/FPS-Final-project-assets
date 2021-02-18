using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    public Gun_raycast gunScript;
    public SwordCombat swordScript;
    public Rigidbody rb;
    public BoxCollider collision;
    public Transform player, weaponSlot1, weaponSlot2, MeleeSlot, playerCamera;

    public float pickUpRange;

    public bool isEquipped;

    private void Start()
    {
        if (!isEquipped)
        {
            gunScript.enabled = false;
            swordScript.enabled = false;
            rb.isKinematic = false;
            collision.isTrigger = false;
        }
        if (isEquipped)
        {
            gunScript.enabled = true;
            swordScript.enabled = false;
            rb.isKinematic = true;
            collision.isTrigger = true;
        }
    }
    private void Update()
    {
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!isEquipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && weaponSlot1.childCount <= 0 && transform.tag != "Sword")
        {
            PickUp();
        }
        if (!isEquipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.Q) && weaponSlot2.childCount <= 0 && transform.tag != "Sword")
        {
            PickUpSecondary();
        }
        if (!isEquipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.M) && MeleeSlot.childCount <= 0 && transform.tag == "Sword")
        {
            PickUpSword();
        }

        if (isEquipped && Input.GetKeyDown(KeyCode.G))
        {
            Drop();
        }
    }

    private void PickUp()
    {
        if (weaponSlot1.childCount <= 0)
        {
            Debug.Log("Slot 1");
            isEquipped = true;
            //slot2Full = true;

            rb.isKinematic = true;
            collision.isTrigger = true;

            transform.SetParent(weaponSlot1);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.Euler(Vector3.zero);

            gunScript.enabled = true;
        }
    }
    private void PickUpSecondary()
    {
        if (weaponSlot2.childCount <= 0)
        {
            Debug.Log("Slot 2");
            isEquipped = true;
            //slot2Full = true;

            rb.isKinematic = true;
            collision.isTrigger = true;

            transform.SetParent(weaponSlot2);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.Euler(Vector3.zero);

            gunScript.enabled = true;
        }
    }
    private void PickUpSword()
    {
        if (MeleeSlot.childCount <= 0)
        {
            Debug.Log("Melee Slot");
            isEquipped = true;
            //slot2Full = true;

            rb.isKinematic = true;
            collision.isTrigger = true;

            transform.SetParent(MeleeSlot);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.Euler(Vector3.zero);

            gunScript.enabled = true;
            swordScript.enabled = true;
        }
    }
    private void Drop()
    {
        isEquipped = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        collision.isTrigger = false;

        gunScript.enabled = false;
        swordScript.enabled = false;
    }
}
