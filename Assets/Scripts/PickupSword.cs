using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (!isEquipped)
        {
            swordScript.enabled = false;
            animator.enabled = false;
            rb.isKinematic = false;
            collision.isTrigger = false;
        }
        if (isEquipped)
        {
            swordScript.enabled = true;
            animator.enabled = true;
            rb.isKinematic = true;
            collision.isTrigger = true;
        }
    }
    private void Update()
    {
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!isEquipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.M) && meleeSlot.childCount <= 0 && transform.tag == "Sword")
        {
            PickUp();
        }
        if (isEquipped && Input.GetKeyDown(KeyCode.G))
        {
            Drop();
        }
    }

    private void PickUp()
    {
        if (meleeSlot.childCount <= 0)
        {
            Debug.Log("Slot 1");
            isEquipped = true;
            //slot2Full = true;

            rb.isKinematic = true;
            collision.isTrigger = true;

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

        transform.SetParent(null);

        rb.isKinematic = false;
        collision.isTrigger = false;

        swordScript.enabled = false;
        animator.enabled = false;
    }
}
