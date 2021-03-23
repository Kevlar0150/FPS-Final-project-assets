using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupEnergy : MonoBehaviour
{
    public EnergyCannon rlScript;
    public Rigidbody rb;
    public BoxCollider collision;
    public Transform player, weaponSlot1, weaponSlot2,playerCamera;

    public float pickUpRange;

    public bool isEquipped;

    Animator anim;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera = player.GetChild(2).transform;
        weaponSlot1 = player.GetChild(2).GetChild(0).transform;
        weaponSlot2 = player.GetChild(2).GetChild(1).transform;

        anim = GetComponent<Animator>();
        if (!isEquipped)
        {
            rlScript.enabled = false;
            rb.isKinematic = false;
            collision.isTrigger = false;
            anim.enabled = false;
        }
        if (isEquipped)
        {
            rlScript.enabled = true;
            rb.isKinematic = true;
            collision.isTrigger = true;
            anim.enabled = true;
        }
    }
    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera = player.GetChild(2).transform;
        weaponSlot1 = player.GetChild(2).GetChild(0).transform;
        weaponSlot2 = player.GetChild(2).GetChild(1).transform;

        anim = GetComponent<Animator>();
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!isEquipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && weaponSlot1.childCount <= 0 && transform.tag != "Sword")
        {
            PickUp();
        }
        if (!isEquipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.Q) && weaponSlot2.childCount <= 0 && transform.tag != "Sword")
        {
            PickUpSecondary();
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
            isEquipped = true;

            rb.isKinematic = true;
            collision.isTrigger = true;

            transform.SetParent(weaponSlot1);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.Euler(Vector3.zero);

            rlScript.enabled = true;
            anim.enabled = true;
        }
    }
    private void PickUpSecondary()
    {
        if (weaponSlot2.childCount <= 0)
        {
            isEquipped = true;

            rb.isKinematic = true;
            collision.isTrigger = true;

            transform.SetParent(weaponSlot2);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.Euler(Vector3.zero);

            rlScript.enabled = true;
            anim.enabled = true;
        }
    }

    private void Drop()
    {
        isEquipped = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        collision.isTrigger = false;

        rlScript.enabled = false;
        anim.enabled = false;
    }
}

